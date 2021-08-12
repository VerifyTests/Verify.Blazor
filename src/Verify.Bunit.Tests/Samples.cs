#if(DEBUG)
using System.Threading.Tasks;
using BlazorApp;
using Bunit;
using VerifyXunit;
using Xunit;

// Non-nullable field is uninitialized
#pragma warning disable CS8618
[UsesVerify]
public class Samples :
    TestContext
{
    #region BunitComponentTest

    [Fact]
    public Task Component()
    {
        var component = RenderComponent<TestComponent>(
            builder =>
            {
                builder.Add(_ => _.Title, "New Title");
                builder.Add(_ => _.Person, new() { Name = "Sam" });
            });
        return Verifier.Verify(component);
    }

    #endregion

    [Fact]
    public Task Nested()
    {
        var component = RenderComponent<TestComponent>(
            builder =>
            {
                builder.Add(_ => _.Title, "New Title");
                builder.Add(_ => _.Person, new() { Name = "Sam" });
            });

        return Verifier.Verify(
            new
            {
                OtherProp = "Foo",
                Component = component
            });
    }
}
#endif