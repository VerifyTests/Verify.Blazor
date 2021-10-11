using Bunit;
using Newtonsoft.Json;
using VerifyTests;

class RenderedFragmentConverter :
    WriteOnlyJsonConverter<IRenderedFragment>
{
    public override void WriteJson(JsonWriter writer, IRenderedFragment fragment, JsonSerializer serializer, IReadOnlyDictionary<string, object> context)
    {
        writer.WriteStartObject();

        writer.WritePropertyName("Instance");
        serializer.Serialize(writer, ComponentReader.GetInstance(fragment));

        writer.WritePropertyName("Markup");
        serializer.Serialize(writer, fragment.Markup);

        writer.WriteEndObject();
    }
}