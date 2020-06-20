using System.Threading.Tasks;
using Verify.Blazor.Tests;
using VerifyTests.Blazor;
using VerifyXunit;
using Xunit;

// Non-nullable field is uninitialized
#pragma warning disable CS8618
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
