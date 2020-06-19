using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Verify.Blazor.Tests;
using VerifyTests;
using VerifyTests.Blazor;
using VerifyXunit;
using Xunit;

// Non-nullable field is uninitialized
#pragma warning disable CS8618
[UsesVerify]
public class Samples
{
    static Samples()
    {
        #region Enable
        VerifyBlazor.Initialize();
        #endregion
    }

    #region ComponentTest
    [Fact]
    public async Task Component()
    {
        var services = new ServiceCollection();
        await using var provider = services.BuildServiceProvider();
        var testRenderer = new TestRenderer(provider, new NullLoggerFactory());

        var result = new RenderedComponent<TestComponent>(testRenderer);
        await result.SetParametersAndRender(ParameterView.Empty);
        var items = result.GetMarkup();
        await Verifier.Verify(items);
    }

    #endregion

}