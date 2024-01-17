using BlazorApp;
using Microsoft.AspNetCore.Components;
using VerifyTests.Blazor;
using Counter = BlazorServerApp.Pages.Counter;

public class Samples
{
    [Fact]
    public Task BlazorServer()
    {
        var target = Render.Component<Counter>();

        return Verify(target);
    }

    #region BlazorComponentTestWithParameters

    [Fact]
    public Task PassingParameters()
    {
        var parameters = ParameterView.FromDictionary(
            new Dictionary<string, object?>
            {
                {
                    "Title", "The Title"
                },
                {
                    "Person", new Person
                    {
                        Name = "Sam"
                    }
                }
            });

        var target = Render.Component<TestComponent>(parameters: parameters);

        return Verify(target);
    }

    #endregion

    #region BlazorComponentTestWithTemplateInstance

    [Fact]
    public Task PassingTemplateInstance()
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

        return Verify(target);
    }

    #endregion

    [Fact]
    public Task Callback()
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

        return Verify(target);
    }
}