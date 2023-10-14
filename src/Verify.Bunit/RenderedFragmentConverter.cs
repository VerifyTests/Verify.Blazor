using AngleSharp;
using Bunit.Diffing;

class RenderedFragmentConverter : WriteOnlyJsonConverter<IRenderedFragment>
{
    public override void Write(VerifyJsonWriter writer, IRenderedFragment fragment)
    {
        var instance = ComponentReader.GetInstance(fragment);
        writer.WriteStartObject();
        writer.WriteMember(fragment, instance, instance is not null ? PrettyName(instance.GetType()) : "Instance");
        writer.WriteMember(fragment, fragment.Nodes.ToHtml(new DiffMarkupFormatter()).Trim(), "Markup");
        writer.WriteEndObject();
    }

    private static string PrettyName(Type type)
    {
        if (type.GetGenericArguments().Length == 0)
        {
            return type.Name;
        }

        var genericArguments = type.GetGenericArguments();
        var typeDefinition = type.Name;
        var unmangledName = typeDefinition.Substring(0, typeDefinition.IndexOf("`"));
        return unmangledName + "<" + String.Join(",", genericArguments.Select(PrettyName)) + ">";
    }
}