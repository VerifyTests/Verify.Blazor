using System.IO;
using Bunit;
using VerifyTests;

static class FragmentToStream
{
    public static ConversionResult Convert(IRenderedFragment fragment, VerifySettings settings)
    {
        var stream = new MemoryStream();
        using var writer = stream.BuildLeaveOpenWriter();
        var markup = fragment.Markup;
        writer.WriteLine(markup);

        var instance = ComponentReader.GetInstance(fragment);
        var all = fragment.FindAll("*");
        var info = new FragmentInfo(
            instance,
            fragment.RenderCount,
            all.Count,
            markup.Replace("\r\n","\n").Length.ToString("N0"));
        return new ConversionResult(info, new []{new ConversionStream("png",stream)});
    }
}