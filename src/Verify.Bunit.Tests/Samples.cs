#if(DEBUG)
using BlazorApp;
using Bunit;

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
        var component = RenderComponent<TestComponent>(
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
        var component = RenderComponent<TestComponent>(
            builder =>
            {
                builder.Add(_ => _.Title, "New Title");
                builder.Add(_ => _.Person, new()
                {
                    Name = "Sam"
                });
            });
        await component.WaitForStateAsync(() => component.Instance.Intitialized);
        await Verify(component);
    }
}
#endif