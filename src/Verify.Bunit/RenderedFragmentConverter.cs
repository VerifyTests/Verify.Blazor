using Bunit;

class RenderedFragmentConverter :
    WriteOnlyJsonConverter<IRenderedFragment>
{
    public override void Write(VerifyJsonWriter writer, IRenderedFragment fragment)
    {
        writer.WriteStartObject();

        writer.WriteMember(fragment, ComponentReader.GetInstance(fragment), "Instance");

        writer.WriteMember(fragment, fragment.Markup, "Markup");

        writer.WriteEndObject();
    }
}