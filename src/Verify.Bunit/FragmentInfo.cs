class FragmentInfo
{
    public object? Instance { get; }
    public int RenderCount { get; }
    public int NodeCount { get; }
    public string Bytes { get; }

    public FragmentInfo(object? instance, int renderCount, int nodeCount, string bytes)
    {
        Instance = instance;
        RenderCount = renderCount;
        NodeCount = nodeCount;
        Bytes = bytes;
    }
}