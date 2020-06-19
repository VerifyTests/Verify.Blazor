﻿using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using VerifyTests.Blazor;

namespace VerifyTests
{
    public static class VerifyBlazor
    {
        static MethodInfo stateHasChanged = typeof(ComponentBase).GetMethod("StateHasChanged", BindingFlags.Instance | BindingFlags.NonPublic);

        public static void Initialize()
        {
            VerifierSettings.RegisterFileConverter<Render>(
                "html",
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
            await root.RenderComponentUnderTest(type, parameters);
            var (componentId, component) = root.FindComponentUnderTest();
            if (target.BeforeRender != null)
            {
                target.BeforeRender(component);
                await root.renderHandle.Dispatcher.InvokeAsync(() => { stateHasChanged.Invoke(component, null); });
                await root.RenderComponentUnderTest(type, parameters);
            }

            var html = Htmlizer.GetHtml(renderer, componentId);

            var stream = new MemoryStream();
            await using var writer = stream.BuildLeaveOpenWriter();
            writer.WriteLine(html);

            return new ConversionResult(new ComponentInfo(component), stream);
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
}

class ComponentInfo
{
    public ComponentBase Instance { get; }

    public ComponentInfo(ComponentBase instance)
    {
        Instance = instance;
    }
}