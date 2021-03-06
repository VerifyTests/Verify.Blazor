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

<a href='https://dotnetfoundation.org' alt='Part of the .NET Foundation'><img src='https://raw.githubusercontent.com/VerifyTests/Verify/master/docs/dotNetFoundation.svg' height='30px'></a><br>
Part of the <a href='https://dotnetfoundation.org' alt=''>.NET Foundation</a>


## Component

The below samples use the following Component:

<!-- snippet: BlazorApp/TestComponent.razor -->
<a id='snippet-BlazorApp/TestComponent.razor'></a>
```razor
<div>
    <h1>@Title</h1>
    <button>MyButton</button>
</div>

@code {
    [Parameter]
    public string Title { get; set; } = "My Test Component";
}
```
<sup><a href='/src/BlazorApp/TestComponent.razor#L1-L9' title='Snippet source file'>snippet source</a> | <a href='#snippet-BlazorApp/TestComponent.razor' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


## Verify.Blazor

Verify.Blazor uses the Blazor APIs to take a snapshot (metadata and html) of the current state of a Blazor component. It has fewer dependencies and is a simpler API than [Verify.Bunit approach](#verifybunit), however it does not provide many of the other features, for example [trigger event handlers](https://bunit.egilhansen.com/docs/interaction/trigger-event-handlers.html).


### NuGet package

 * https://nuget.org/packages/Verify.Blazor/


### Usage

Enable at startup:

<!-- snippet: ModuleInitializer.cs -->
<a id='snippet-ModuleInitializer.cs'></a>
```cs
using System.Runtime.CompilerServices;
using ImageMagick;
using Verify.AngleSharp;
using VerifyTests;

public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Initialize()
    {
        // remove some noise from the html snapshot


        VerifierSettings.ScrubEmptyLines();
        VerifierSettings.ScrubLinesWithReplace(s => s.Replace("<!--!-->", ""));
        HtmlPrettyPrint.All();
        VerifierSettings.ScrubLinesContaining("<script src=\"_framework/dotnet.");


        VerifyPlaywright.Enable();
        VerifyImageMagick.RegisterComparers(
            threshold: .01,
            metric: ErrorMetric.MeanAbsolute);
    }
}
```
<sup><a href='/src/Verify.Blazor.Tests/ModuleInitializer.cs#L1-L25' title='Snippet source file'>snippet source</a> | <a href='#snippet-ModuleInitializer.cs' title='Start of snippet'>anchor</a></sup>
<a id='snippet-ModuleInitializer.cs-1'></a>
```cs
using System.Runtime.CompilerServices;
using VerifyTests;
using VerifyXunit;


[UsesVerify]
public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Initialize()
    {
        VerifyBunit.Initialize();
    }
}
```
<sup><a href='/src/Verify.Bunit.Tests/ModuleInitializer.cs#L1-L15' title='Snippet source file'>snippet source</a> | <a href='#snippet-ModuleInitializer.cs-1' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

This test:

<!-- snippet: BlazorComponentTest -->
<a id='snippet-blazorcomponenttest'></a>
```cs
[Fact]
public async Task Component()
{
    var target = Render.Component<TestComponent>();
    await Verifier.Verify(target);
}
```
<sup><a href='/src/Verify.Blazor.Tests/Samples.cs#L10-L19' title='Snippet source file'>snippet source</a> | <a href='#snippet-blazorcomponenttest' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

Will produce:

The component rendered as html `...Component.01.verified.html`:

<!-- snippet: Verify.Blazor.Tests/Samples.Component.01.verified.html -->
<a id='snippet-Verify.Blazor.Tests/Samples.Component.01.verified.html'></a>
```html
<div>
  <h1>My Test Component</h1>
  <button>MyButton</button>
</div>
```
<sup><a href='/src/Verify.Blazor.Tests/Samples.Component.01.verified.html#L1-L5' title='Snippet source file'>snippet source</a> | <a href='#snippet-Verify.Blazor.Tests/Samples.Component.01.verified.html' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

And the current model rendered as txt `...Component.00.verified.txt`:

<!-- snippet: Verify.Blazor.Tests/Samples.Component.00.verified.txt -->
<a id='snippet-Verify.Blazor.Tests/Samples.Component.00.verified.txt'></a>
```txt
{
  Instance: {
    Title: My Test Component
  },
  Bytes: 67
}
```
<sup><a href='/src/Verify.Blazor.Tests/Samples.Component.00.verified.txt#L1-L6' title='Snippet source file'>snippet source</a> | <a href='#snippet-Verify.Blazor.Tests/Samples.Component.00.verified.txt' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### BeforeRender

The state of the component can optionally be manipulated before it is rendered.

This test:

<!-- snippet: BeforeRender -->
<a id='snippet-beforerender'></a>
```cs
[Fact]
public async Task BeforeRender()
{
    var target = Render.Component<TestComponent>(
        beforeRender: component => { component.Title = "New Title"; });
    await Verifier.Verify(target);
}
```
<sup><a href='/src/Verify.Blazor.Tests/Samples.cs#L21-L31' title='Snippet source file'>snippet source</a> | <a href='#snippet-beforerender' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

Will produce:

<!-- snippet: Verify.Blazor.Tests/Samples.BeforeRender.01.verified.html -->
<a id='snippet-Verify.Blazor.Tests/Samples.BeforeRender.01.verified.html'></a>
```html
<div>
  <h1>New Title</h1>
  <button>MyButton</button>
</div>
```
<sup><a href='/src/Verify.Blazor.Tests/Samples.BeforeRender.01.verified.html#L1-L5' title='Snippet source file'>snippet source</a> | <a href='#snippet-Verify.Blazor.Tests/Samples.BeforeRender.01.verified.html' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

And

<!-- snippet: Verify.Blazor.Tests/Samples.BeforeRender.00.verified.txt -->
<a id='snippet-Verify.Blazor.Tests/Samples.BeforeRender.00.verified.txt'></a>
```txt
{
  Instance: {
    Title: New Title
  },
  Bytes: 59
}
```
<sup><a href='/src/Verify.Blazor.Tests/Samples.BeforeRender.00.verified.txt#L1-L6' title='Snippet source file'>snippet source</a> | <a href='#snippet-Verify.Blazor.Tests/Samples.BeforeRender.00.verified.txt' title='Start of snippet'>anchor</a></sup>
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
[UsesVerify]
public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Initialize()
    {
        VerifyBunit.Initialize();
    }
}
```
<sup><a href='/src/Verify.Bunit.Tests/ModuleInitializer.cs#L5-L17' title='Snippet source file'>snippet source</a> | <a href='#snippet-bunitenable' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

This test:

<!-- snippet: BunitComponentTest -->
<a id='snippet-bunitcomponenttest'></a>
```cs
[Fact]
public Task Component()
{
    var component = RenderComponent<TestComponent>();
    return Verifier.Verify(component);
}
```
<sup><a href='/src/Verify.Bunit.Tests/Samples.cs#L14-L22' title='Snippet source file'>snippet source</a> | <a href='#snippet-bunitcomponenttest' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

Will produce:

The component rendered as html `...Component.01.verified.html`:

<!-- snippet: Verify.Bunit.Tests/Samples.Component.01.verified.html -->
<a id='snippet-Verify.Bunit.Tests/Samples.Component.01.verified.html'></a>
```html
<div><h1>My Test Component</h1>
    <button>MyButton</button></div>
```
<sup><a href='/src/Verify.Bunit.Tests/Samples.Component.01.verified.html#L1-L2' title='Snippet source file'>snippet source</a> | <a href='#snippet-Verify.Bunit.Tests/Samples.Component.01.verified.html' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

And the current model rendered as txt `...Component.00.verified.txt`:

<!-- snippet: Verify.Bunit.Tests/Samples.Component.00.verified.txt -->
<a id='snippet-Verify.Bunit.Tests/Samples.Component.00.verified.txt'></a>
```txt
{
  Instance: {
    Title: My Test Component
  },
  RenderCount: 1,
  NodeCount: 3,
  Bytes: 67
}
```
<sup><a href='/src/Verify.Bunit.Tests/Samples.Component.00.verified.txt#L1-L8' title='Snippet source file'>snippet source</a> | <a href='#snippet-Verify.Bunit.Tests/Samples.Component.00.verified.txt' title='Start of snippet'>anchor</a></sup>
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
VerifierSettings.ScrubEmptyLines();
VerifierSettings.ScrubLinesWithReplace(s => s.Replace("<!--!-->", ""));
HtmlPrettyPrint.All();
VerifierSettings.ScrubLinesContaining("<script src=\"_framework/dotnet.");
```
<sup><a href='/src/Verify.Blazor.Tests/ModuleInitializer.cs#L13-L20' title='Snippet source file'>snippet source</a> | <a href='#snippet-scrubbers' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


## Credits

 * [Unit testing Blazor components - a prototype - Steven Sanderson](https://blog.stevensanderson.com/2019/08/29/blazor-unit-testing-prototype/)
 * [Bunit - Egil Hansen](https://bunit.egilhansen.com)



## Icon

[Helmet](https://thenounproject.com/term/helmet/9554/) designed by [Leonidas Ikonomou](https://thenounproject.com/alterego) from [The Noun Project](https://thenounproject.com).
