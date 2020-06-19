using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace VerifyTests.Blazor
{
    public class TestRenderer : Renderer
    {
        Exception? unhandledException;

        TaskCompletionSource<object?> nextRenderCompletion = new TaskCompletionSource<object?>();

        public TestRenderer(IServiceProvider services, ILoggerFactory? loggerFactory = null)
            : base(services, loggerFactory?? new NullLoggerFactory())
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