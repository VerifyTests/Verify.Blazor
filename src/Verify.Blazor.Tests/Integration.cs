using System.Diagnostics;
using System.Threading.Tasks;
using Verify.Blazor.Tests;
using VerifyTests;
using VerifyTests.Blazor;
using VerifyXunit;
using Xunit;

[UsesVerify]
public class Integration
{
    [Fact]
    public async Task Component()
    {
        VerifySelenium.Enable();

        Process.Start("dotnet")
        var target = Render.Component<TestComponent>();
        await Verifier.Verify(target);
    }

    [Fact]
    public async Task BeforeRender()
    {
        var target = Render.Component<TestComponent>(
            beforeRender: component => { component.Title = "New Title"; });
        await Verifier.Verify(target);
    }
}