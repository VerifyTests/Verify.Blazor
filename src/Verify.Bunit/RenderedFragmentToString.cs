static class RenderedFragmentToString
{
    public static ConversionResult Convert(IRenderedFragment fragment, IReadOnlyDictionary<string, object> context)
    {
        var nodes = fragment.Nodes;
        var markup = nodes
            .ToHtml(new DiffMarkupFormatter())
            .Trim();
        var nodeCount = nodes.Sum(_ => _
            .GetDescendantsAndSelf()
            .Count());
        var info = new FragmentInfo(ComponentReader.GetInstance(fragment), nodeCount);
        return new(info, "html", markup);
    }
}