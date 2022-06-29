class FragmentInfo
{
    public object? Instance { get; }
    public int NodeCount { get; }

    public FragmentInfo(object? instance, int nodeCount)
    {
        Instance = instance;
        NodeCount = nodeCount;
    }
}