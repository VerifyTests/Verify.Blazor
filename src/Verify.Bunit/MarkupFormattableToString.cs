using AngleSharp;
using Bunit.Diffing;

static class MarkupFormattableToString
{
    public static ConversionResult Convert(IMarkupFormattable markup, IReadOnlyDictionary<string, object> context)
        => new ConversionResult(null, "html", markup.ToHtml(new DiffMarkupFormatter()).Trim());
}