class TestRenderer(IServiceProvider services, ILoggerFactory logger) :
    Renderer(services, logger)
{
    Exception? unhandledException;

    TaskCompletionSource<object?> nextRenderCompletion = new();

    public new ArrayRange<RenderTreeFrame> GetCurrentRenderTreeFrames(int id)
        => base.GetCurrentRenderTreeFrames(id);

    public int AttachTestRootComponent(ContainerComponent root)
        => AssignRootComponentId(root);

    public override Dispatcher Dispatcher { get; } = Dispatcher.CreateDefault();

    protected override void HandleException(Exception exception) =>
        unhandledException = exception;

    protected override Task UpdateDisplayAsync(in RenderBatch batch)
    {
        // TODO: Capture batches (and the state of component output) for individual inspection
        var prevTcs = nextRenderCompletion;
        nextRenderCompletion = new();
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
            ExceptionDispatchInfo
                .Capture(unhandledException)
                .Throw();
        }
    }
}