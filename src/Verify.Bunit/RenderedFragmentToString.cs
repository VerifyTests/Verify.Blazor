using AngleSharp;
using AngleSharp.Dom;
using Bunit.Diffing;

static class RenderedFragmentToString
{
    public static ConversionResult Convert(IRenderedFragment fragment, IReadOnlyDictionary<string, object> context)
    {
        var markup = fragment.Nodes.ToHtml(new DiffMarkupFormatter()).Trim();
        var allNodes = fragment.Nodes.Select(x => x.GetDescendantsAndSelf().Count()).Sum();
        var info = new FragmentInfo(ComponentReader.GetInstance(fragment), allNodes);
        return new ConversionResult(info, "html", markup);
    }
}