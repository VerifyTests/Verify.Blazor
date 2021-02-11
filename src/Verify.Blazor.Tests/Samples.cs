#if(DEBUG)
using System.Threading.Tasks;
using BlazorApp;
using VerifyTests.Blazor;
using VerifyXunit;
using Xunit;

[UsesVerify]
public class Samples
{
    #region BlazorComponentTest

    [Fact]
    public async Task Component()
    {
        var target = Render.Component<TestComponent>();
        await Verifier.Verify(target);
    }

    #endregion

    #region BeforeRender

    [Fact]
    public async Task BeforeRender()
    {
        var target = Render.Component<TestComponent>(
            beforeRender: component => { component.Title = "New Title"; });
        await Verifier.Verify(target);
    }

    #endregion
}
#endif