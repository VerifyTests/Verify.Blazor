class RenderedFragmentConverter :
    WriteOnlyJsonConverter<IRenderedFragment>
{
    public override void Write(VerifyJsonWriter writer, IRenderedFragment fragment)
    {
        writer.WriteStartObject();

        var instance = ComponentReader.GetInstance(fragment);
        if (instance != null)
        {
            writer.WriteMember(fragment, instance, PrettyName(instance.GetType()));
        }

        writer.WriteMember(fragment, fragment.Nodes.ToHtml(new DiffMarkupFormatter()).Trim(), "Markup");
        writer.WriteEndObject();
    }

    static string PrettyName(Type type)
    {
        var genericArguments = type.GetGenericArguments();
        if (genericArguments.Length == 0)
        {
            return type.Name;
        }

        var typeName = type.Name;
        var unmangledName = typeName[..typeName.IndexOf('`')];
        return $"{unmangledName}<{string.Join(',', genericArguments.Select(PrettyName))}>";
    }
}