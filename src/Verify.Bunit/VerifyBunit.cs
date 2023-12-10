using Bunit.Extensions.WaitForHelpers;
using Microsoft.AspNetCore.Components;

namespace VerifyTests;

public static class VerifyBunit
{
    public static bool Initialized { get; private set; }

    public static void Initialize(bool excludeComponent = false)
    {
        if (Initialized)
        {
            throw new("Already Initialized");
        }

        Initialized = true;

        InnerVerifier.ThrowIfVerifyHasBeenRun();

        if (excludeComponent)
        {
            VerifierSettings.RegisterFileConverter<IRenderedFragment>(RenderedFragmentMarkupToString.Convert);
        }
        else
        {
            VerifierSettings.RegisterFileConverter<IRenderedFragment>(RenderedFragmentToString.Convert);
        }

        VerifierSettings.RegisterFileConverter<IMarkupFormattable>(MarkupFormattableToString.Convert);

        VerifierSettings.AddExtraSettings(_ => _.Converters.Add(new RenderedFragmentConverter()));
        VerifierSettings.AddExtraSettings(_ => _.Converters.Add(new MarkupFormattableConverter()));
        VerifierSettings.RegisterStringComparer("html", BunitMarkupComparer.Compare);
    }

    /// <summary>
    /// Wait until the provided <paramref name="predicate" /> action returns true,
    /// or the <paramref name="timeout" /> is reached (default is one second).
    /// The <paramref name="predicate" /> is evaluated initially, and then each time
    /// the <paramref name="fragment" /> renders.
    /// </summary>
    /// <param name="fragment">The render fragment or component to attempt to verify state against.</param>
    /// <param name="predicate">The predicate to invoke after each render, which must returns <c>true</c> when the desired state has been reached.</param>
    /// <param name="timeout">The maximum time to wait for the desired state.</param>
    /// <exception cref="WaitForFailedException">Thrown if the <paramref name="predicate" /> throw an exception during invocation, or if the timeout has been reached. See the inner exception for details.</exception>
    public static async Task WaitFor(this IRenderedFragmentBase fragment, Func<bool> predicate, TimeSpan? timeout = null)
    {
        using var waiter = new WaitForStateHelper(fragment, predicate, timeout);
        await waiter.WaitTask;
    }

    public static Task ClickAsync(
        this IElement element,
        long detail = 1,
        double screenX = default,
        double screenY = default,
        double clientX = default,
        double clientY = default,
        long button = default,
        long buttons = default,
        bool ctrlKey = default,
        bool shiftKey = default,
        bool altKey = default,
        bool metaKey = default,
        string? type = default) =>
        MouseEventDispatchExtensions.ClickAsync(element,
            new()
            {
                Detail = detail,
                ScreenX = screenX,
                ScreenY = screenY,
                ClientX = clientX,
                ClientY = clientY,
                Button = button,
                Buttons = buttons,
                CtrlKey = ctrlKey,
                ShiftKey = shiftKey,
                AltKey = altKey,
                MetaKey = metaKey,
                Type = type!
            });

    public static Task ChangeAsync<T>(this IElement element, T value) =>
        InputEventDispatchExtensions.ChangeAsync(element, CreateFrom(value));

    static ChangeEventArgs CreateFrom<T>(T value) =>
        new()
        {
            Value = FormatValue(value)
        };

    static object? FormatValue<T>(T value)
        => value switch
        {
            null => null,
            bool _ => value,
            string _ => value,
            ICollection values => FormatValues(values),
            IEnumerable values => FormatValues(values),
            _ => BindConverter.FormatValue(value)
        };

    static object?[] FormatValues(ICollection values)
    {
        var result = new object?[values.Count];

        var index = 0;
        foreach (var value in values)
        {
            result[index++] = FormatValue(value);
        }

        return result;
    }

    static object?[] FormatValues(IEnumerable values)
    {
        var result = new List<object?>();

        foreach (var value in values)
        {
            result.Add(FormatValue(value));
        }

        return result.ToArray();
    }

    /// <summary>
    /// Instantiates and performs a first render of a component of type <typeparamref name="TComponent" />.
    /// </summary>
    /// <typeparam name="TComponent">Type of the component to render.</typeparam>
    /// <param name="context">The <see cref="TestContext" /> to extend.</param>
    /// <param name="parameters">Parameters to pass to the component when it is rendered.</param>
    /// <param name="renderedCheck">Checks if rendered has finished.</param>
    /// <returns>The rendered <typeparamref name="TComponent" />.</returns>
    public static Task<IRenderedComponent<TComponent>> RenderComponentAndWait<TComponent>(
        this TestContext context,
        Func<TComponent, bool> renderedCheck,
        params ComponentParameter[] parameters)
        where TComponent : IComponent =>
        Inner(() => context.RenderComponent<TComponent>(parameters), null, renderedCheck);

    /// <summary>
    /// Instantiates and performs a first render of a component of type <typeparamref name="TComponent" />.
    /// </summary>
    /// <typeparam name="TComponent">Type of the component to render.</typeparam>
    /// <param name="context">The <see cref="TestContext" /> to extend.</param>
    /// <param name="parameterBuilder">The ComponentParameterBuilder action to add type safe parameters to pass to the component when it is rendered.</param>
    /// <param name="timeout">A TimeSpan that represents the to wait, or null to use 10 seconds.</param>
    /// <param name="renderedCheck">Checks if rendered has finished.</param>
    /// <returns>The rendered <typeparamref name="TComponent" />.</returns>
    public static Task<IRenderedComponent<TComponent>> RenderComponentAndWait<TComponent>(
        this TestContext context,
        Action<ComponentParameterCollectionBuilder<TComponent>> parameterBuilder,
        Func<TComponent, bool> renderedCheck,
        TimeSpan? timeout = null)
        where TComponent : IComponent =>
        Inner(() => context.RenderComponent(parameterBuilder), timeout, renderedCheck);

    /// <summary>
    /// Renders the <paramref name="fragment" /> and returns the first <typeparamref name="TComponent" /> in the resulting render tree.
    /// </summary>
    /// <remarks>
    /// Calling this method is equivalent to calling <c>Render(renderFragment).FindComponent&lt;TComponent&gt;()</c>.
    /// </remarks>
    /// <typeparam name="TComponent">The type of component to find in the render tree.</typeparam>
    /// <param name="fragment">The render fragment to render.</param>
    /// <param name="context">The <see cref="TestContext" /> to extend.</param>
    /// <param name="renderedCheck">Checks if rendered has finished.</param>
    /// <param name="timeout">A TimeSpan that represents the to wait, or null to use 10 seconds.</param>
    /// <returns>The <see cref="IRenderedComponent{TComponent}" />.</returns>
    public static Task<IRenderedComponent<TComponent>> RenderAndWait<TComponent>(
        this TestContext context,
        RenderFragment fragment,
        Func<TComponent, bool> renderedCheck,
        TimeSpan? timeout = null)
        where TComponent : IComponent =>
        Inner(() => context.Render<TComponent>(fragment), timeout, renderedCheck);

    static async Task<IRenderedComponent<TComponent>> Inner<TComponent>(
        Func<IRenderedComponent<TComponent>> render,
        TimeSpan? timeout,
        Func<TComponent, bool> renderedCheck)
        where TComponent : IComponent
    {
        var iterations = timeout?.Milliseconds / 10 ?? 100;
        var target = render();

        var instance = target.Instance;
        while (!renderedCheck(instance))
        {
            if (iterations-- == 0)
            {
                throw new TimeoutException("Timeout waiting for component to render");
            }

            await Task.Delay(10);
        }

        return target;
    }
}