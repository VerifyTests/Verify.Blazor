using AngleSharp.Dom;
using Bunit;
using Bunit.Extensions.WaitForHelpers;
using Microsoft.AspNetCore.Components;

namespace VerifyTests;

public static class VerifyBunit
{
    public static void Initialize()
    {
        VerifierSettings.RegisterFileConverter<IRenderedFragment>(FragmentToStream.Convert);
        VerifierSettings.AddExtraSettings(
            _ => _.Converters.Add(new RenderedFragmentConverter()));
    }

    /// <summary>
    /// Wait until the provided <paramref name="predicate"/> action returns true,
    /// or the <paramref name="timeout"/> is reached (default is one second).
    ///
    /// The <paramref name="predicate"/> is evaluated initially, and then each time
    /// the <paramref name="fragment"/> renders.
    /// </summary>
    /// <param name="fragment">The render fragment or component to attempt to verify state against.</param>
    /// <param name="predicate">The predicate to invoke after each render, which must returns <c>true</c> when the desired state has been reached.</param>
    /// <param name="timeout">The maximum time to wait for the desired state.</param>
    /// <exception cref="WaitForFailedException">Thrown if the <paramref name="predicate"/> throw an exception during invocation, or if the timeout has been reached. See the inner exception for details.</exception>
    public static async Task WaitFor(this IRenderedFragmentBase fragment, Func<bool> predicate, TimeSpan? timeout = null)
    {
        using var waiter = new WaitForStateHelper(fragment, predicate, timeout);
        await waiter.WaitTask;
    }

    public static Task ClickAsync(this IElement element, long detail = 1, double screenX = default, double screenY = default, double clientX = default, double clientY = default, long button = default, long buttons = default, bool ctrlKey = default, bool shiftKey = default, bool altKey = default, bool metaKey = default, string? type = default) =>
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
            String _ => value,
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
    /// Instantiates and performs a first render of a component of type <typeparamref name="TComponent"/>.
    /// </summary>
    /// <typeparam name="TComponent">Type of the component to render.</typeparam>
    /// <param name="context">The <see cref="TestContext"/> to extend.</param>
    /// <param name="parameters">Parameters to pass to the component when it is rendered.</param>
    /// <returns>The rendered <typeparamref name="TComponent"/>.</returns>
    public static IRenderedComponent<TComponent> RenderComponentAndWait<TComponent>(
        this TestContext context, params ComponentParameter[] parameters)
        where TComponent : IComponent =>
        RenderAndWait(() => context.RenderComponent<TComponent>(parameters), null);

    /// <summary>
    /// Instantiates and performs a first render of a component of type <typeparamref name="TComponent"/>.
    /// </summary>
    /// <typeparam name="TComponent">Type of the component to render.</typeparam>
    /// <param name="context">The <see cref="TestContext"/> to extend.</param>
    /// <param name="parameterBuilder">The ComponentParameterBuilder action to add type safe parameters to pass to the component when it is rendered.</param>
    /// <param name="timeout">A TimeSpan that represents the to wait, or null to use 10 seconds.</param>
    /// <returns>The rendered <typeparamref name="TComponent"/>.</returns>
    public static IRenderedComponent<TComponent> RenderComponentAndWait<TComponent>(
        this TestContext context,
        Action<ComponentParameterCollectionBuilder<TComponent>> parameterBuilder,
        TimeSpan? timeout = null)
        where TComponent : IComponent =>
        RenderAndWait(() => context.RenderComponent(parameterBuilder), timeout);

    /// <summary>
    /// Renders the <paramref name="fragment"/> and returns the first <typeparamref name="TComponent"/> in the resulting render tree.
    /// </summary>
    /// <remarks>
    /// Calling this method is equivalent to calling <c>Render(renderFragment).FindComponent&lt;TComponent&gt;()</c>.
    /// </remarks>
    /// <typeparam name="TComponent">The type of component to find in the render tree.</typeparam>
    /// <param name="fragment">The render fragment to render.</param>
    /// <param name="context">The <see cref="TestContext"/> to extend.</param>
    /// <param name="timeout">A TimeSpan that represents the to wait, or null to use 10 seconds.</param>
    /// <returns>The <see cref="IRenderedComponent{TComponent}"/>.</returns>
    public static IRenderedComponent<TComponent> RenderAndWait<TComponent>(
        this TestContext context,
        RenderFragment fragment,
        TimeSpan? timeout = null)
        where TComponent : IComponent =>
        RenderAndWait(() => context.Render<TComponent>(fragment), timeout);


    /// <summary>
    /// Renders the <paramref name="fragment"/> and returns it as a <see cref="IRenderedFragment"/>.
    /// </summary>
    /// <param name="fragment">The render fragment to render.</param>
    /// <param name="context">The <see cref="TestContext"/> to extend.</param>
    /// <param name="timeout">A TimeSpan that represents the to wait, or null to use 10 seconds.</param>
    /// <returns>The <see cref="IRenderedFragment"/>.</returns>
    public static IRenderedFragment RenderAndWait(this TestContext context, RenderFragment fragment, TimeSpan? timeout = null) =>
        RenderAndWait(() => context.Render(fragment), timeout);

    static T RenderAndWait<T>(Func<T> render, TimeSpan? timeout)
        where T : IRenderedFragmentBase
    {
        using var wait = new ManualResetEventSlim(false);
        EventHandler handler = (_, _) => wait?.Set();
        var target = render();
        target.OnAfterRender += handler;
        wait.WaitHandle.WaitOne(timeout ?? TimeSpan.FromSeconds(10));
        target.OnAfterRender -= handler;
        return target;
    }
}