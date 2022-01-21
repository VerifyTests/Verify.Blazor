using Bunit;
using Newtonsoft.Json;

class RenderedFragmentConverter :
    WriteOnlyJsonConverter<IRenderedFragment>
{
    public override void Write(VerifyJsonWriter writer, IRenderedFragment fragment, JsonSerializer serializer)
    {
        writer.WriteStartObject();

        writer.WritePropertyName("Instance");
        serializer.Serialize(writer, ComponentReader.GetInstance(fragment));

        writer.WriteProperty(fragment, _ => _.Markup);

        writer.WriteEndObject();
    }
}