using System.IO;
using System.Text;

static class StreamHelpers
{
    static UTF8Encoding utf8NoBOM = new(false, true);

    public static MemoryStream ToStream(this string input)
    {
        return new(utf8NoBOM.GetBytes(input));
    }
}