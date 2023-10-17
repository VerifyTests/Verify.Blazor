static class MarkupFormattableToString
{
    public static ConversionResult Convert(IMarkupFormattable markup, IReadOnlyDictionary<string, object> context) =>
        new(null, "html", markup.ToHtml(new DiffMarkupFormatter()).Trim());
}