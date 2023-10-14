using AngleSharp;
using AngleSharp.Diffing.Core;
using AngleSharp.Dom;
using AngleSharp.Text;
using Bunit.Diffing;
using Bunit.Rendering;
using Microsoft.AspNetCore.Components;
using System.Runtime.CompilerServices;

static class BunitMarkupComparer
{
    public static Task<VerifyTests.CompareResult> Compare(string received, string verified, IReadOnlyDictionary<string, object> context)
    {
        using var parser = new BunitHtmlParser();
        var receivedNodes = parser.Parse(received);
        var verifiedNodes = parser.Parse(verified);
        var diffs = receivedNodes.CompareTo(verifiedNodes);

        var result = diffs.Count == 0
            ? VerifyTests.CompareResult.Equal
            : VerifyTests.CompareResult.NotEqual(CreateDiffMessage(received, verified, diffs));

        return Task.FromResult(result);
    }

    private static string CreateDiffMessage(string received, string verified, IReadOnlyList<IDiff> diffs)
    {
        var builder = StringBuilderPool.Obtain();
        builder.AppendLine();
        builder.AppendLine("HTML comparison failed. The following errors were found:");

        for (int i = 0; i < diffs.Count; i++)
        {
            builder.Append($"  {i + 1}: ");
            builder.AppendLine(diffs[i] switch
            {
                NodeDiff diff when diff.Target == DiffTarget.Text && diff.Control.Path.Equals(diff.Test.Path, StringComparison.Ordinal)
                    => $"The text in {diff.Control.Path} is different.",
                NodeDiff diff when diff.Target == DiffTarget.Text
                    => $"The expected {NodeName(diff.Control)} at {diff.Control.Path} and the actual {NodeName(diff.Test)} at {diff.Test.Path} is different.",
                NodeDiff diff when diff.Control.Path.Equals(diff.Test.Path, StringComparison.Ordinal)
                    => $"The {NodeName(diff.Control)}s at {diff.Control.Path} are different.",
                NodeDiff diff => $"The expected {NodeName(diff.Control)} at {diff.Control.Path} and the actual {NodeName(diff.Test)} at {diff.Test.Path} are different.",
                AttrDiff diff when diff.Control.Path.Equals(diff.Test.Path, StringComparison.Ordinal)
                    => $"The values of the attributes at {diff.Control.Path} are different.",
                AttrDiff diff => $"The value of the attribute {diff.Control.Path} and actual attribute {diff.Test.Path} are different.",
                MissingNodeDiff diff => $"The {NodeName(diff.Control)} at {diff.Control.Path} is missing.",
                MissingAttrDiff diff => $"The attribute at {diff.Control.Path} is missing.",
                UnexpectedNodeDiff diff => $"The {NodeName(diff.Test)} at {diff.Test.Path} was not expected.",
                UnexpectedAttrDiff diff => $"The attribute at {diff.Test.Path} was not expected.",
                _ => throw new SwitchExpressionException($"Unknown diff type detected: {diffs[i].GetType()}"),
            });
        }

        builder.AppendLine();
        builder.AppendLine("Actual HTML:");
        builder.AppendLine();
        builder.AppendLine(received);
        builder.AppendLine();
        builder.AppendLine("Expected HTML:");
        builder.AppendLine();
        builder.AppendLine(verified);

        return builder.ToPool();

        static string NodeName(ComparisonSource source) => source.Node.NodeType.ToString().ToLowerInvariant();
    }
}