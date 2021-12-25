using BlazorApp;
using Microsoft.AspNetCore.Components;
using VerifyTests.Blazor;

[UsesVerify]
public class Samples
{
    #region BlazorComponentTestWithParameters

    [Fact]
    public async Task PassingParameters()
    {
        var parameters = ParameterView.FromDictionary(
            new Dictionary<string, object?>
            {
                { "Title", "The Title" },
                { "Person", new Person { Name = "Sam" } }
            });

        var target = Render.Component<TestComponent>(parameters: parameters);

        await Verify(target);
    }

    #endregion

    #region BlazorComponentTestWithTemplateInstance

    [Fact]
    public async Task PassingTemplateInstance()
    {
        var template = new TestComponent
        {
            Title = "The Title",
            Person = new()
            {
                Name = "Sam"
            }
        };

        var target = Render.Component(template: template);

        await Verify(target);
    }

    #endregion


    [Fact]
    public async Task Callback()
    {
        var template = new TestComponent
        {
            Title = "The Title",
            Person = new()
            {
                Name = "Sam"
            }
        };

        var target = Render.Component(
            template: template,
            callback: component =>
            {
                Assert.NotSame(component, template);
                component.Title = "New title";
            });

        await Verify(target);
    }
}