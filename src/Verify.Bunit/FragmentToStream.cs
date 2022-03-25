using Bunit;

static class FragmentToStream
{
    public static ConversionResult Convert(IRenderedFragment fragment, IReadOnlyDictionary<string, object> context)
    {
        var markup = fragment.Markup.Replace("\r\n", "\n");

        var instance = ComponentReader.GetInstance(fragment);
        var all = fragment.FindAll("*");
        var info = new FragmentInfo(
            instance,
            all.Count,
            markup.Length.ToString("N0"));
        return new(info, "html", markup);
    }
}