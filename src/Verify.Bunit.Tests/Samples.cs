#if (Debug)
using System.Threading.Tasks;
using Bunit;
using Verify.Bunit.Tests;
using VerifyTests;
using VerifyXunit;
using Xunit;

// Non-nullable field is uninitialized
#pragma warning disable CS8618
[UsesVerify]
public class Samples :
    TestContext
{
    static Samples()
    {
        #region BunitEnable
        VerifyBunit.Initialize();
        #endregion
    }

    #region BunitComponentTest
    [Fact]
    public Task Component()
    {
        var component = RenderComponent<TestComponent>();
        return Verifier.Verify(component);
    }

    #endregion

    #region BunitBeforeRender
    [Fact]
    public Task BeforeRender()
    {
        var component = RenderComponent<TestComponent>();
        component.Instance.Title = "New Title";
        component.Render();
        return Verifier.Verify(component);
    }

    #endregion

    [Fact]
    public Task Nested()
    {
        var component = RenderComponent<TestComponent>();

        return Verifier.Verify(
            new
            {
                OtherProp = "Foo",
                Component = component
            });
    }
}
#endif