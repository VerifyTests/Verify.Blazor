<!--
GENERATED FILE - DO NOT EDIT
This file was generated by [MarkdownSnippets](https://github.com/SimonCropp/MarkdownSnippets).
Source File: /readme.source.md
To change this file edit the source file and then run MarkdownSnippets.
-->

# <img src="/src/icon.png" height="30px"> Verify.Blazor

[![Build status](https://ci.appveyor.com/api/projects/status/spyere4ubpl1tca8?svg=true)](https://ci.appveyor.com/project/SimonCropp/Verify-Blazor)
[![NuGet Status](https://img.shields.io/nuget/v/Verify.Bunit.svg?label=Verify.Bunit)](https://www.nuget.org/packages/Verify.Bunit/)
[![NuGet Status](https://img.shields.io/nuget/v/Verify.Blazor.svg?label=Verify.Blazor)](https://www.nuget.org/packages/Verify.Blazor/)

Support for rendering a [Blazor Component](https://docs.microsoft.com/en-us/aspnet/core/blazor/#components) to a verified file via [bunit](https://bunit.egilhansen.com) or via raw Blazor rendering.



## Component

The below samples use the following Component:

<!-- snippet: BlazorApp/TestComponent.razor -->
<a id='snippet-BlazorApp/TestComponent.razor'></a>
```razor
<div>
    <h1>@Title</h1>
    <p>@Person.Name</p>
    <button>MyButton</button>
</div>

@code {
    [Parameter]
    public string Title { get; set; } = "My Test Component";

    [Parameter]
    public Person Person { get; set; }
}
```
<sup><a href='/src/BlazorApp/TestComponent.razor#L1-L13' title='Snippet source file'>snippet source</a> | <a href='#snippet-BlazorApp/TestComponent.razor' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


## Verify.Blazor

Verify.Blazor uses the Blazor APIs to take a snapshot (metadata and html) of the current state of a Blazor component. It has fewer dependencies and is a simpler API than [Verify.Bunit approach](#verifybunit), however it does not provide many of the other features, for example [trigger event handlers](https://bunit.egilhansen.com/docs/interaction/trigger-event-handlers.html).


### NuGet package

 * https://nuget.org/packages/Verify.Blazor/


### Usage

#### Render using ParameterView

This test:

<!-- snippet: BlazorComponentTestWithParameters -->
<a id='snippet-blazorcomponenttestwithparameters'></a>
```cs
[Fact]
public Task PassingParameters()
{
    var parameters = ParameterView.FromDictionary(
        new Dictionary<string, object?>
        {
            { "Title", "The Title" },
            { "Person", new Person { Name = "Sam" } }
        });

    var target = Render.Component<TestComponent>(parameters: parameters);

    return Verify(target);
}
```
<sup><a href='/src/Verify.Blazor.Tests/Samples.cs#L16-L33' title='Snippet source file'>snippet source</a> | <a href='#snippet-blazorcomponenttestwithparameters' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


#### Render using template instance

This test:

<!-- snippet: BlazorComponentTestWithTemplateInstance -->
<a id='snippet-blazorcomponenttestwithtemplateinstance'></a>
```cs
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
```
<sup><a href='/src/Verify.Blazor.Tests/Samples.cs#L35-L54' title='Snippet source file'>snippet source</a> | <a href='#snippet-blazorcomponenttestwithtemplateinstance' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


#### Result

Both will produce:

The component rendered as html `...01.verified.html`:

<!-- snippet: Verify.Blazor.Tests/Samples.PassingParameters.01.verified.html -->
<a id='snippet-Verify.Blazor.Tests/Samples.PassingParameters.01.verified.html'></a>
```html
<div>
  <h1>The Title</h1>
  <p>Sam</p>
  <button>MyButton</button>
</div>
```
<sup><a href='/src/Verify.Blazor.Tests/Samples.PassingParameters.01.verified.html#L1-L6' title='Snippet source file'>snippet source</a> | <a href='#snippet-Verify.Blazor.Tests/Samples.PassingParameters.01.verified.html' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

And the current model rendered as txt `...00.verified.txt`:

<!-- snippet: Verify.Blazor.Tests/Samples.PassingParameters.00.verified.txt -->
<a id='snippet-Verify.Blazor.Tests/Samples.PassingParameters.00.verified.txt'></a>
```txt
{
  Instance: {
    Title: The Title,
    Person: {
      Name: Sam
    }
  },
  Bytes: 74
}
```
<sup><a href='/src/Verify.Blazor.Tests/Samples.PassingParameters.00.verified.txt#L1-L9' title='Snippet source file'>snippet source</a> | <a href='#snippet-Verify.Blazor.Tests/Samples.PassingParameters.00.verified.txt' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


## Verify.Bunit

Verify.Bunit uses the bUnit APIs to take a snapshot (metadata and html) of the current state of a Blazor component. Since it leverages the bUnit API, snapshots can be on a component that has been manipulated using the full bUnit feature set, for example [trigger event handlers](https://bunit.egilhansen.com/docs/interaction/trigger-event-handlers.html).


### NuGet package

 * https://nuget.org/packages/Verify.Bunit/


### Usage

Enable at startup:

<!-- snippet: BunitEnable -->
<a id='snippet-bunitenable'></a>
```cs
public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Initialize()
    {
        VerifyBunit.Initialize();
    }
}
```
<sup><a href='/src/Verify.Bunit.Tests/ModuleInitializer.cs#L1-L12' title='Snippet source file'>snippet source</a> | <a href='#snippet-bunitenable' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

This test:

<!-- snippet: BunitComponentTest -->
<a id='snippet-bunitcomponenttest'></a>
```cs
[Fact]
public Task Component()
{
    var component = RenderComponent<TestComponent>(
        builder =>
        {
            builder.Add(_ => _.Title, "New Title");
            builder.Add(_ => _.Person, new() { Name = "Sam" });
        });
    return Verify(component);
}
```
<sup><a href='/src/Verify.Bunit.Tests/Samples.cs#L11-L25' title='Snippet source file'>snippet source</a> | <a href='#snippet-bunitcomponenttest' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

Will produce:

The component rendered as html `...Component.01.verified.html`:

<!-- snippet: Verify.Bunit.Tests/Samples.Component.01.verified.html -->
<a id='snippet-Verify.Bunit.Tests/Samples.Component.01.verified.html'></a>
```html
<div><h1>New Title</h1>
    <p>Sam</p>
    <button>MyButton</button></div>
```
<sup><a href='/src/Verify.Bunit.Tests/Samples.Component.01.verified.html#L1-L3' title='Snippet source file'>snippet source</a> | <a href='#snippet-Verify.Bunit.Tests/Samples.Component.01.verified.html' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

And the current model rendered as txt `...Component.00.verified.txt`:

<!-- snippet: Verify.Bunit.Tests/Samples.Component.00.verified.txt -->
<a id='snippet-Verify.Bunit.Tests/Samples.Component.00.verified.txt'></a>
```txt
{
  Instance: {
    Title: New Title,
    Person: {
      Name: Sam
    }
  },
  RenderCount: 1,
  NodeCount: 4,
  Bytes: 74
}
```
<sup><a href='/src/Verify.Bunit.Tests/Samples.Component.00.verified.txt#L1-L11' title='Snippet source file'>snippet source</a> | <a href='#snippet-Verify.Bunit.Tests/Samples.Component.00.verified.txt' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


## Scrubbing


### Integrity check

In Blazor an integrity check is applied to the `dotnet.*.js` file.

```
<script src="_framework/dotnet.5.0.2.js" defer="" integrity="sha256-AQfZ6sKmq4EzOxN3pymKJ1nlGQaneN66/2mcbArnIJ8=" crossorigin="anonymous"></script>
```

This line will change when the dotnet SDK is updated.


### Pretty print

For readability it is useful to pretty print html using [Verify.AngleSharp](https://github.com/VerifyTests/Verify.AngleSharp#pretty-print).


### Noise in rendered template

Blazor uses `<!--!-->` to delineate components in the resulting html. Some empty lines can be rendered when components are stitched together.


### Resulting scrubbing

<!-- snippet: scrubbers -->
<a id='snippet-scrubbers'></a>
```cs
// remove some noise from the html snapshot
VerifierSettings.ScrubEmptyLines();
VerifierSettings.ScrubLinesWithReplace(s => s.Replace("<!--!-->", ""));
HtmlPrettyPrint.All();
VerifierSettings.ScrubLinesContaining("<script src=\"_framework/dotnet.");
```
<sup><a href='/src/Verify.Blazor.Tests/ModuleInitializer.cs#L9-L17' title='Snippet source file'>snippet source</a> | <a href='#snippet-scrubbers' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


## Credits

 * [Unit testing Blazor components - a prototype - Steven Sanderson](https://blog.stevensanderson.com/2019/08/29/blazor-unit-testing-prototype/)
 * [Bunit - Egil Hansen](https://bunit.egilhansen.com)



## Icon

[Helmet](https://thenounproject.com/term/helmet/9554/) designed by [Leonidas Ikonomou](https://thenounproject.com/alterego) from [The Noun Project](https://thenounproject.com).
