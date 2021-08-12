using System;
using System.Collections.Generic;
using System.Reflection;
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
        internal ParameterView Parameters { get; }
        internal Type Type { get; }

        Render(
            Type type,
            ServiceProvider? provider,
            ILoggerFactory? loggerFactory,
            ParameterView parameters)
        {
            Provider = provider;
            LoggerFactory = loggerFactory;
            Parameters = parameters;
            Type = type;
        }

        public static Render Component<T>(
            ServiceProvider? provider = null,
            ILoggerFactory? loggerFactory = null,
            ParameterView? parameters = null,
            T? template = null)
            where T : ComponentBase
        {
            return new(
                typeof(T),
                provider,
                loggerFactory,
                Merge(parameters, template));
        }

        static ParameterView Merge<T>(ParameterView? parameters, T? template)
            where T : ComponentBase
        {
            if (parameters == null &&
                template is null)
            {
                return ParameterView.Empty;
            }

            if (template is null)
            {
                return parameters!.Value;
            }
            
            var dictionary = new Dictionary<string, object>();
            if (parameters != null)
            {
                foreach (var item in parameters)
                {
                    dictionary.Add(item.Name, item.Value);
                }
            }

            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.FlattenHierarchy;
            foreach (var property in typeof(T).GetProperties(flags))
            {
                var value = property.GetValue(template);
                if (value != null)
                {
                    dictionary.Add(property.Name, value);
                }
            }

            return ParameterView.FromDictionary(dictionary);
        }
    }
}