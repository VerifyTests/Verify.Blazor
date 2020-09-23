using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using VerifyTests;
using VerifyTests.Blazor;

static class VerifyBlazor
{
    static MethodInfo stateHasChanged = typeof(ComponentBase)
        .GetMethod("StateHasChanged", BindingFlags.Instance | BindingFlags.NonPublic)!;

    public static void Initialize()
    {
        VerifierSettings.RegisterFileConverter<Render>(
            async (target, settings) => await RenderToResult(target));
    }

    static async Task<ConversionResult> RenderToResult(Render target)
    {
        await using var provider = GetProvider(target);
        var loggerFactory = target.LoggerFactory ?? NullLoggerFactory.Instance;
        var renderer = new TestRenderer(provider, loggerFactory);

        var root = new ContainerComponent(renderer);
        var parameters = target.ParameterView ?? ParameterView.Empty;

        var type = target.Type;
        var dispatcher = root.RenderHandle.Dispatcher;
        await root.RenderComponentUnderTest(type, parameters);
        var (componentId, component) = root.FindComponentUnderTest();
        if (target.BeforeRender != null)
        {
            target.BeforeRender(component);
            await dispatcher.InvokeAsync(() => { stateHasChanged.Invoke(component, null); });
            await root.RenderComponentUnderTest(type, parameters);
        }

        var html = Htmlizer.GetHtml(renderer, componentId);

        var stream = html.ToStream();

        var info = new ComponentInfo(component, html.Replace("\r\n", "\n").Length.ToString("N0"));
        return new ConversionResult(
            info,
            new List<ConversionStream>
            {
                new ConversionStream("html", stream)
            });
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