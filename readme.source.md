# <img src="/src/icon.png" height="30px"> Verify.Blazor

[![Discussions](https://img.shields.io/badge/Verify-Discussions-yellow?svg=true&label=)](https://github.com/orgs/VerifyTests/discussions)
[![Build status](https://ci.appveyor.com/api/projects/status/spyere4ubpl1tca8?svg=true)](https://ci.appveyor.com/project/SimonCropp/Verify-Blazor)
[![NuGet Status](https://img.shields.io/nuget/v/Verify.Blazor.svg?label=Verify.Blazor)](https://www.nuget.org/packages/Verify.Blazor/)

Support for rendering a [Blazor Component](https://docs.microsoft.com/en-us/aspnet/core/blazor/#components) to a verified file via Blazor rendering.


Verify.Blazor uses the Blazor APIs to take a snapshot (metadata and html) of the current state of a Blazor component. It has fewer dependencies and is a simpler API than [Verify.Bunit approach](https://github.com/VerifyTests/Verify.Bunit), however it does not provide many of the other features, for example [trigger event handlers](https://bunit.egilhansen.com/docs/interaction/trigger-event-handlers.html).

**See [Milestones](../../milestones?state=closed) for release notes.**

## Component

The below samples use the following Component:

snippet: BlazorApp/TestComponent.razor

## NuGet package

* https://nuget.org/packages/Verify.Blazor/

## Usage

### Render using ParameterView

This test:

snippet: BlazorComponentTestWithParameters

### Render using template instance

This test:

snippet: BlazorComponentTestWithTemplateInstance

### Result

Both will produce:

The component rendered as html `...verified.html`:

snippet: Verify.Blazor.Tests/Samples.PassingParameters.verified.html

And the current model rendered as txt `...verified.txt`:

snippet: Verify.Blazor.Tests/Samples.PassingParameters.verified.txt


## Scrubbing


### Integrity check

In Blazor an integrity check is applied to the `dotnet.*.js` file.

```
<script src="_framework/dotnet.5.0.2.js" defer="" integrity="sha256-AQfZ6sKmq4EzOxN3pymKJ1nlGQaneN66/2mcbArnIJ8=" crossorigin="anonymous"></script>
```

This line will change when the dotnet SDK is updated.


### Noise in rendered template

Blazor uses `<!--!-->` to delineate components in the resulting html. Some empty lines can be rendered when components are stitched together.


### Resulting scrubbing

snippet: scrubbers


## Icon

[Helmet](https://thenounproject.com/term/helmet/9554/) designed
by [Leonidas Ikonomou](https://thenounproject.com/alterego) from [The Noun Project](https://thenounproject.com).