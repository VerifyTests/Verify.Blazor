using System.Collections.Generic;
using Bunit;
using VerifyTests;

static class FragmentToStream
{
    public static ConversionResult Convert(IRenderedFragment fragment, IReadOnlyDictionary<string, object> context)
    {
        var markup = fragment.Markup.Replace("\r\n", "\n");

        var instance = ComponentReader.GetInstance(fragment);
        var all = fragment.FindAll("*");
        FragmentInfo info = new(
            instance,
            fragment.RenderCount,
            all.Count,
            markup.Length.ToString("N0"));
        return new(info, "html", markup);
    }
}