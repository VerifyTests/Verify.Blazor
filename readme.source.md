# <img src="/src/icon.png" height="30px"> Verify.Blazor

[![Build status](https://ci.appveyor.com/api/projects/status/spyere4ubpl1tca8?svg=true)](https://ci.appveyor.com/project/SimonCropp/Verify-Blazor)
[![NuGet Status](https://img.shields.io/nuget/v/Verify.Bunit.svg?label=Verify.Bunit)](https://www.nuget.org/packages/Verify.Bunit/)
[![NuGet Status](https://img.shields.io/nuget/v/Verify.Blazor.svg?label=Verify.Blazor)](https://www.nuget.org/packages/Verify.Blazor/)

Support for rendering a [Blazor Component](https://docs.microsoft.com/en-us/aspnet/core/blazor/#components) to a verified file via [bunit](https://bunit.egilhansen.com) or via raw Blazor rendering.

Support is available via a [Tidelift Subscription](https://tidelift.com/subscription/pkg/nuget-verify?utm_source=nuget-verify&utm_medium=referral&utm_campaign=enterprise).

toc


## NuGet packages

 * https://nuget.org/packages/Verify.Bunit/
 * https://nuget.org/packages/Verify.Blazor/


## Verify.Blazor Usage

Enable at startup:

snippet: BlazorEnable

Given the following Component:

snippet: Verify.Blazor.Tests/TestComponent.razor

This test:

snippet: BlazorComponentTest

Will produce:

The component rendered as html `...Component.verified.html`:

snippet: Verify.Blazor.Tests/Samples.Component.verified.html

And the current model rendered as txt `...Component.info.verified.txt`:

snippet: Verify.Blazor.Tests/Samples.Component.info.verified.txt

### BeforeRender

The state of the componetn can optopnal be manipulated before it is rendered.

This test:

snippet: BeforeRender

Will produce:

snippet: Verify.Blazor.Tests/Samples.BeforeRender.verified.html

And

snippet: Verify.Blazor.Tests/Samples.BeforeRender.info.verified.txt


## Verify.Bunit Usage

Enable at startup:

snippet: BunitEnable

Given the following Component:

snippet: Verify.Bunit.Tests/TestComponent.razor

This test:

snippet: BunitComponentTest

Will produce:

The component rendered as html `...Component.verified.html`:

snippet: Verify.Bunit.Tests/Samples.Component.verified.html

And the current model rendered as txt `...Component.info.verified.txt`:

snippet: Verify.Bunit.Tests/Samples.Component.info.verified.txt


## Security contact information

To report a security vulnerability, use the [Tidelift security contact](https://tidelift.com/security). Tidelift will coordinate the fix and disclosure.


## Icon

[Helmet](https://thenounproject.com/term/helmet/9554/) designed by [Leonidas Ikonomou](https://thenounproject.com/alterego) from [The Noun Project](https://thenounproject.com).