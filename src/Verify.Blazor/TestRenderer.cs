using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.Extensions.Logging;

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

public class ContainerComponent : IComponent
{
    TestRenderer renderer;
    RenderHandle renderHandle;
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

    public (int componentId, TComponent component) FindComponentUnderTest<TComponent>()
    {
        var ownFrames = renderer.GetCurrentRenderTreeFrames(componentId);
        if (ownFrames.Count == 0)
        {
            throw new InvalidOperationException($"{nameof(ContainerComponent)} hasn't yet rendered");
        }

        ref var childComponentFrame = ref ownFrames.Array[0];
        return (childComponentFrame.ComponentId, (TComponent)childComponentFrame.Component);
    }

    public Task RenderComponentUnderTest<TComponent>(ParameterView parameters)
        where TComponent : IComponent
    {
        return renderer.DispatchAndAssertNoSynchronousErrors(() =>
        {
            renderHandle.Render(builder =>
            {
                builder.OpenComponent(0, typeof(TComponent));

                foreach (var parameterValue in parameters)
                {
                    builder.AddAttribute(1, parameterValue.Name, parameterValue.Value);
                }

                builder.CloseComponent();
            });
        });
    }
}
    public class TestRenderer : Renderer
    {
        Exception? unhandledException;

        TaskCompletionSource<object?> nextRenderCompletion = new TaskCompletionSource<object?>();

        public TestRenderer(IServiceProvider services, ILoggerFactory loggerFactory)
            : base(services, loggerFactory)
        {
        }

        public new ArrayRange<RenderTreeFrame> GetCurrentRenderTreeFrames(int componentId)
            => base.GetCurrentRenderTreeFrames(componentId);

        public int AttachTestRootComponent(ContainerComponent testRootComponent)
            => AssignRootComponentId(testRootComponent);

        public new Task DispatchEventAsync(ulong handlerId, EventFieldInfo fieldInfo, EventArgs args)
        {
            var task = Dispatcher.InvokeAsync(
                () => base.DispatchEventAsync(handlerId, fieldInfo, args));
            AssertNoSynchronousErrors();
            return task;
        }

        public override Dispatcher Dispatcher { get; } = Dispatcher.CreateDefault();

        public Task NextRender => nextRenderCompletion.Task;

        protected override void HandleException(Exception exception)
        {
            unhandledException = exception;
        }

        protected override Task UpdateDisplayAsync(in RenderBatch renderBatch)
        {
            // TODO: Capture batches (and the state of component output) for individual inspection
            var prevTcs = nextRenderCompletion;
            nextRenderCompletion = new TaskCompletionSource<object?>();
            prevTcs.SetResult(null);
            return Task.CompletedTask;
        }

        public async Task DispatchAndAssertNoSynchronousErrors(Action callback)
        {
            await Dispatcher.InvokeAsync(callback);
            AssertNoSynchronousErrors();
        }

        void AssertNoSynchronousErrors()
        {
            if (unhandledException != null)
            {
                ExceptionDispatchInfo.Capture(unhandledException).Throw();
            }
        }
    }
}