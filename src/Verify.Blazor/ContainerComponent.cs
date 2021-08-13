using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

// This provides the ability for test code to trigger rendering at arbitrary times,
// and to supply arbitrary parameters to the component being tested (including ones
// flagged as 'cascading').
//
// This also avoids the use of Renderer's RenderRootComponentAsync APIs, which are
// not a good entry-point for unit tests, because their asynchrony is all about waiting
// for quiescence. We don't want that in tests because we want to assert about all
// possible states, including loading states.
class ContainerComponent :
    IComponent
{
    TestRenderer renderer;
    internal RenderHandle RenderHandle;
    int componentId;

    public ContainerComponent(TestRenderer renderer)
    {
        this.renderer = renderer;
        componentId = renderer.AttachTestRootComponent(this);
    }

    public void Attach(RenderHandle renderHandle)
    {
        RenderHandle = renderHandle;
    }

    public Task SetParametersAsync(ParameterView parameters)
    {
        throw new($"{nameof(ContainerComponent)} shouldn't receive any parameters");
    }

    public (int componentId, ComponentBase component) FindComponentUnderTest()
    {
        var frames = renderer.GetCurrentRenderTreeFrames(componentId);
        if (frames.Count == 0)
        {
            throw new($"{nameof(ContainerComponent)} hasn't yet rendered");
        }

        ref var frame = ref frames.Array[0];
        return (frame.ComponentId, (ComponentBase)frame.Component);
    }

    public Task RenderComponentUnderTest(Type type, ParameterView parameters)
    {
        return renderer.DispatchAndAssertNoSynchronousErrors(
            () => { RenderHandle.Render(builder => { Render(type, parameters, builder); }); });
    }

    static void Render(Type type, ParameterView parameters, RenderTreeBuilder builder)
    {
        builder.OpenComponent(0, type);

        foreach (var parameterValue in parameters)
        {
            builder.AddAttribute(1, parameterValue.Name, parameterValue.Value);
        }

        builder.CloseComponent();
    }
}