using System;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace VerifyTests.Blazor
{
    public class Render
    {
        static Render()
        {
            VerifyBlazor.Initialize();
        }
        internal ServiceProvider? Provider { get; }
        internal ILoggerFactory? LoggerFactory { get; }
        internal ParameterView? ParameterView { get; }
        internal Action<ComponentBase>? BeforeRender { get; }
        internal Type Type { get; }

        Render(
            Type type,
            ServiceProvider? provider = null,
            ILoggerFactory? loggerFactory = null,
            ParameterView? parameterView = null,
            Action<ComponentBase>? beforeRender = null)
        {
            Provider = provider;
            LoggerFactory = loggerFactory;
            ParameterView = parameterView;
            BeforeRender = beforeRender;
            Type = type;
        }

        public static Render Component<T>(
            ServiceProvider? provider = null,
            ILoggerFactory? loggerFactory = null,
            ParameterView? parameterView = null,
            Action<T>? beforeRender = null)
            where T : ComponentBase
        {
            return new(
                typeof(T),
                provider,
                loggerFactory,
                parameterView,
                component => { beforeRender?.Invoke((T) component); });
        }
    }
}