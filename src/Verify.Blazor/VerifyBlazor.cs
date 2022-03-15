using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using VerifyTests.Blazor;

static class VerifyBlazor
{
    static MethodInfo stateHasChanged = typeof(ComponentBase)
        .GetMethod("StateHasChanged", BindingFlags.Instance | BindingFlags.NonPublic)!;

    public static void Initialize() =>
        VerifierSettings.RegisterFileConverter<Render>(
            (target, _) => RenderToResult(target));

    static async Task<ConversionResult> RenderToResult(Render target)
    {
        await using var provider = GetProvider(target);
        var loggerFactory = target.LoggerFactory ?? NullLoggerFactory.Instance;
        var renderer = new TestRenderer(provider, loggerFactory);

        var root = new ContainerComponent(renderer);

        var type = target.Type;
        var dispatcher = root.RenderHandle.Dispatcher;
        await root.RenderComponentUnderTest(type, target.Parameters);
        var (componentId, component) = root.FindComponentUnderTest();
        if (target.Callback != null)
        {
            target.Callback(component);
            await dispatcher.InvokeAsync(() => { stateHasChanged.Invoke(component, null); });
        }

        var html = Htmlizer.GetHtml(renderer, componentId).Replace("\r\n", "\n");
        var info = new ComponentInfo(component, html.Length.ToString("N0"));
        return new(info, "html", html);
    }

    static ServiceProvider GetProvider(Render target)
    {
        if (target.Provider != null)
        {
            return target.Provider;
        }

        var services = new ServiceCollection();
        return services.BuildServiceProvider();
    }
}