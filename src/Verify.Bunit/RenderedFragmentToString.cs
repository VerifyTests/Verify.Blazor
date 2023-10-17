static class RenderedFragmentToString
{
    public static ConversionResult Convert(IRenderedFragment fragment, IReadOnlyDictionary<string, object> context)
    {
        var markup = fragment.Nodes.ToHtml(new DiffMarkupFormatter()).Trim();
        var allNodes = fragment.Nodes.Select(_ => _.GetDescendantsAndSelf().Count()).Sum();
        var info = new FragmentInfo(ComponentReader.GetInstance(fragment), allNodes);
        return new(info, "html", markup);
    }
}