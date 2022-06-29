using BlazorApp;
using Bunit;

// Non-nullable field is uninitialized
#pragma warning disable CS8618
[UsesVerify]
public class Samples
{
    #region BunitComponentTest

    [Fact]
    public Task Component()
    {
        using var testContext = new TestContext();
        var component = testContext.RenderComponent<TestComponent>(
            builder =>
            {
                builder.Add(_ => _.Title, "New Title");
                builder.Add(_ => _.Person, new()
                {
                    Name = "Sam"
                });
            });
        return Verify(component);
    }

    #endregion

    [Fact]
    public Task Nested()
    {
        using var testContext = new TestContext();
        var component = testContext.RenderComponent<TestComponent>(
            builder =>
            {
                builder.Add(_ => _.Title, "New Title");
                builder.Add(_ => _.Person, new()
                {
                    Name = "Sam"
                });
            });

        return Verify(
            new
            {
                OtherProp = "Foo",
                Component = component
            });
    }

    [Fact]
    public async Task WaitForState()
    {
        using var testContext = new TestContext();
        var component = await testContext.RenderComponentAndWait<TestComponent>(
            builder =>
            {
                builder.Add(_ => _.Title, "New Title");
                builder.Add(_ => _.Person, new()
                {
                    Name = "Sam"
                });
            },
            _ => _.Intitialized);
        await Verify(component);
    }
}