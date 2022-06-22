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
}