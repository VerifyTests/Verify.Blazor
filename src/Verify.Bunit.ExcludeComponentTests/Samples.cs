[UsesVerify]
public class Samples
{
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