using System.IO;
using System.Text;

static class StreamHelpers
{
    static Encoding utf8NoBOM = new UTF8Encoding(false, true);

    public static MemoryStream ToStream(this string input)
    {
        return new MemoryStream(utf8NoBOM.GetBytes(input));
    }
}