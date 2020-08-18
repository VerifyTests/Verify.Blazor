using Bunit;
using VerifyTests;

static class FragmentToStream
{
    public static ConversionResult Convert(IRenderedFragment fragment, VerifySettings settings)
    {
        var markup = fragment.Markup;
        var stream = markup.ToStream();

        var instance = ComponentReader.GetInstance(fragment);
        var all = fragment.FindAll("*");
        var info = new FragmentInfo(
            instance,
            fragment.RenderCount,
            all.Count,
            markup.Replace("\r\n","\n").Length.ToString("N0"));
        return new ConversionResult(info, new []{new ConversionStream("html", stream)});
    }
}