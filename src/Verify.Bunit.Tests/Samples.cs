

// Non-nullable field is uninitialized
#pragma warning disable CS8618
[UsesVerify]
public class Samples
{
    #region BunitComponentTest

    [Fact]
    public Task Component()
    {
        using var context = new TestContext();
        var component = context.RenderComponent<TestComponent>(
            builder =>
            {
                builder.Add(
                    _ => _.Title,
                    "New Title");
                builder.Add(
                    _ => _.Person,
                    new()
                    {
                        Name = "Sam"
                    });
            });
        return Verify(component);
    }

    [Fact]
    public Task MarkupFormattable_NodeList()
    {
        using var context = new TestContext();
        var component = context.RenderComponent<TestComponent>(
            builder =>
            {
                builder.Add(
                    _ => _.Title,
                    "New Title");
                builder.Add(
                    _ => _.Person,
                    new()
                    {
                        Name = "Sam"
                    });
            });
        return Verify(component.Nodes);
    }

    [Fact]
    public Task MarkupFormattable_single_Element()
    {
        using var context = new TestContext();
        var component = context.RenderComponent<TestComponent>(
            builder =>
            {
                builder.Add(
                    _ => _.Title,
                    "New Title");
                builder.Add(
                    _ => _.Person,
                    new()
                    {
                        Name = "Sam"
                    });
            });
        return Verify(component.Nodes.First().FirstChild);
    }

    #endregion

    [Fact]
    public Task Nested()
    {
        using var context = new TestContext();
        var component = context.RenderComponent<TestComponent>(
            builder =>
            {
                builder.Add(
                    _ => _.Title,
                    "New Title");
                builder.Add(
                    _ => _.Person,
                    new()
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
    public Task MarkupFormattable_Nested()
    {
        using var context = new TestContext();
        var component = context.RenderComponent<TestComponent>(
            builder =>
            {
                builder.Add(
                    _ => _.Title,
                    "New Title");
                builder.Add(
                    _ => _.Person,
                    new()
                    {
                        Name = "Sam"
                    });
            });
        return Verify(
            new
            {
                component.Nodes
            });
    }

    [Fact]
    public async Task WaitForState()
    {
        using var context = new TestContext();
        var component = await context.RenderComponentAndWait<TestComponent>(
            builder =>
            {
                builder.Add(
                    _ => _.Title,
                    "New Title");
                builder.Add(
                    _ => _.Person,
                    new()
                    {
                        Name = "Sam"
                    });
            },
            _ => _.Intitialized);
        await Verify(component);
    }
}