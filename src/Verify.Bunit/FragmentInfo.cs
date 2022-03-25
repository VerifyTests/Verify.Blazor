class FragmentInfo
{
    public object? Instance { get; }
    public int NodeCount { get; }
    public string Bytes { get; }

    public FragmentInfo(object? instance, int nodeCount, string bytes)
    {
        Instance = instance;
        NodeCount = nodeCount;
        Bytes = bytes;
    }
}