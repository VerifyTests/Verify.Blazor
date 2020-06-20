using Microsoft.AspNetCore.Components;

class ComponentInfo
{
    public ComponentBase Instance { get; }
    public string Bytes { get; }

    public ComponentInfo(ComponentBase instance, string bytes)
    {
        Instance = instance;
        Bytes = bytes;
    }
}