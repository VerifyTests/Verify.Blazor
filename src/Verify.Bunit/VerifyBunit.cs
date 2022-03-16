using Bunit;
using Bunit.Extensions.WaitForHelpers;

namespace VerifyTests;

public static class VerifyBunit
{
    public static void Initialize()
    {
        VerifierSettings.RegisterFileConverter<IRenderedFragment>(FragmentToStream.Convert);
        VerifierSettings.ModifySerialization(settings =>
        {
            settings.AddExtraSettings(serializerSettings =>
            {
                var converters = serializerSettings.Converters;
                converters.Add(new RenderedFragmentConverter());
            });
        });
    }

    /// <summary>
    /// Wait until the provided <paramref name="statePredicate"/> action returns true,
    /// or the <paramref name="timeout"/> is reached (default is one second).
    ///
    /// The <paramref name="statePredicate"/> is evaluated initially, and then each time
    /// the <paramref name="renderedFragment"/> renders.
    /// </summary>
    /// <param name="renderedFragment">The render fragment or component to attempt to verify state against.</param>
    /// <param name="statePredicate">The predicate to invoke after each render, which must returns <c>true</c> when the desired state has been reached.</param>
    /// <param name="timeout">The maximum time to wait for the desired state.</param>
    /// <exception cref="WaitForFailedException">Thrown if the <paramref name="statePredicate"/> throw an exception during invocation, or if the timeout has been reached. See the inner exception for details.</exception>
    public static async Task WaitForStateAsync(this IRenderedFragmentBase fragment, Func<bool> predicate, TimeSpan? timeout = null)
    {
        using var waiter = new WaitForStateHelper(fragment, predicate, timeout);
        await waiter.WaitTask;
    }
}