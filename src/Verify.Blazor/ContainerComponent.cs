using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace VerifyTests.Blazor
{
    // This provides the ability for test code to trigger rendering at arbitrary times,
    // and to supply arbitrary parameters to the component being tested (including ones
    // flagged as 'cascading').
    //
    // This also avoids the use of Renderer's RenderRootComponentAsync APIs, which are
    // not a good entry-point for unit tests, because their asynchrony is all about waiting
    // for quiescence. We don't want that in tests because we want to assert about all
    // possible states, including loading states.
    public class ContainerComponent :
        IComponent
    {
        TestRenderer renderer;
        internal RenderHandle renderHandle;
        int componentId;

        public ContainerComponent(TestRenderer renderer)
        {
            this.renderer = renderer;
            componentId = renderer.AttachTestRootComponent(this);
        }

        public void Attach(RenderHandle renderHandle)
        {
            this.renderHandle = renderHandle;
        }

        public Task SetParametersAsync(ParameterView parameters)
        {
            throw new NotImplementedException($"{nameof(ContainerComponent)} shouldn't receive any parameters");
        }

        public (int componentId, ComponentBase component) FindComponentUnderTest()
        {
            var ownFrames = renderer.GetCurrentRenderTreeFrames(componentId);
            if (ownFrames.Count == 0)
            {
                throw new InvalidOperationException($"{nameof(ContainerComponent)} hasn't yet rendered");
            }

            ref var childComponentFrame = ref ownFrames.Array[0];
            return (childComponentFrame.ComponentId, (ComponentBase)childComponentFrame.Component);
        }

        public Task RenderComponentUnderTest(Type type, ParameterView parameters)
        {
            return renderer.DispatchAndAssertNoSynchronousErrors(() =>
            {
                renderHandle.Render(builder =>
                {
                    builder.OpenComponent(0,type);

                    foreach (var parameterValue in parameters)
                    {
                        builder.AddAttribute(1, parameterValue.Name, parameterValue.Value);
                    }

                    builder.CloseComponent();
                });
            });
        }
    }
}