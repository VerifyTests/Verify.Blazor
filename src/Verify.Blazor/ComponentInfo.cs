using Microsoft.AspNetCore.Components;

class ComponentInfo
{
    public ComponentBase Instance { get; }

    public ComponentInfo(ComponentBase instance) =>
        Instance = instance;
}