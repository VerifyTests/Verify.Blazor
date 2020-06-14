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
        #region Enable
        VerifyBunit.Initialize();
        #endregion
    }

    #region ComponentTest
    [Fact]
    public Task Component()
    {
        var component = RenderComponent<TestComponent>();
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