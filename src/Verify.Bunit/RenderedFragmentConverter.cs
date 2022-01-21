using Bunit;

class RenderedFragmentConverter :
    WriteOnlyJsonConverter<IRenderedFragment>
{
    public override void Write(VerifyJsonWriter writer, IRenderedFragment fragment)
    {
        writer.WriteStartObject();

        writer.WriteProperty(fragment, ComponentReader.GetInstance(fragment), "Instance");

        writer.WriteProperty(fragment, fragment.Markup, "Markup");

        writer.WriteEndObject();
    }
}