using AngleSharp;
using Bunit.Diffing;

class MarkupFormattableConverter : WriteOnlyJsonConverter<IMarkupFormattable>
{
    public override void Write(VerifyJsonWriter writer, IMarkupFormattable markup)
    {
        writer.WriteStartObject();
        writer.WriteMember(markup, markup.ToHtml(new DiffMarkupFormatter()).Trim(), "Markup");
        writer.WriteEndObject();
    }
}