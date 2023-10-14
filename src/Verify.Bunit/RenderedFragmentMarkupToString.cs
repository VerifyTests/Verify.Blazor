using AngleSharp;
using Bunit.Diffing;

static class RenderedFragmentMarkupToString
{
    public static ConversionResult Convert(IRenderedFragment fragment, IReadOnlyDictionary<string, object> context)
    {
        var markup = fragment.Nodes.ToHtml(new DiffMarkupFormatter()).Trim();
        return new ConversionResult(null, "html", markup);
    }
}
