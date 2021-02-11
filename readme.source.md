# <img src="/src/icon.png" height="30px"> Verify.Blazor

[![Build status](https://ci.appveyor.com/api/projects/status/spyere4ubpl1tca8?svg=true)](https://ci.appveyor.com/project/SimonCropp/Verify-Blazor)
[![NuGet Status](https://img.shields.io/nuget/v/Verify.Bunit.svg?label=Verify.Bunit)](https://www.nuget.org/packages/Verify.Bunit/)
[![NuGet Status](https://img.shields.io/nuget/v/Verify.Blazor.svg?label=Verify.Blazor)](https://www.nuget.org/packages/Verify.Blazor/)

Support for rendering a [Blazor Component](https://docs.microsoft.com/en-us/aspnet/core/blazor/#components) to a verified file via [bunit](https://bunit.egilhansen.com) or via raw Blazor rendering.

Support is available via a [Tidelift Subscription](https://tidelift.com/subscription/pkg/nuget-verify?utm_source=nuget-verify&utm_medium=referral&utm_campaign=enterprise).

<a href='https://dotnetfoundation.org' alt='Part of the .NET Foundation'><img src='https://raw.githubusercontent.com/VerifyTests/Verify/master/docs/dotNetFoundation.svg' height='30px'></a><br>
Part of the <a href='https://dotnetfoundation.org' alt=''>.NET Foundation</a>

toc

## Component

The below samples use the following Component:

snippet: BlazorApp/TestComponent.razor


## Verify.Blazor

Verify.Blazor uses the Blazor APIs to take a snapshot (metadata and html) of the current state of a Blazor component. It has fewer dependencies and is a simpler API than [Verify.Bunit approach](#verifybunit), however it does not provide many of the other features, for example [trigger event handlers](https://bunit.egilhansen.com/docs/interaction/trigger-event-handlers.html).


### NuGet package

 * https://nuget.org/packages/Verify.Blazor/


### Usage

This test:

snippet: BlazorComponentTest

Will produce:

The component rendered as html `...Component.01.verified.html`:

snippet: Verify.Blazor.Tests/Samples.Component.01.verified.html

And the current model rendered as txt `...Component.00.verified.txt`:

snippet: Verify.Blazor.Tests/Samples.Component.00.verified.txt


### BeforeRender

The state of the component can optionally be manipulated before it is rendered.

This test:

snippet: BeforeRender

Will produce:

snippet: Verify.Blazor.Tests/Samples.BeforeRender.01.verified.html

And

snippet: Verify.Blazor.Tests/Samples.BeforeRender.00.verified.txt


## Verify.Bunit

Verify.Bunit uses the bUnit APIs to take a snapshot (metadata and html) of the current state of a Blazor component. Since it leverages the bUnit API, snapshots can be on a component that has been manipulated using the full bUnit feature set, for example [trigger event handlers](https://bunit.egilhansen.com/docs/interaction/trigger-event-handlers.html).


### NuGet package

 * https://nuget.org/packages/Verify.Bunit/


### Usage

Enable at startup:

snippet: BunitEnable

This test:

snippet: BunitComponentTest

Will produce:

The component rendered as html `...Component.verified.html`:

snippet: Verify.Bunit.Tests/Samples.Component.verified.html

And the current model rendered as txt `...Component.info.verified.txt`:

snippet: Verify.Bunit.Tests/Samples.Component.info.verified.txt


## Credits

 * [Unit testing Blazor components - a prototype - Steven Sanderson](https://blog.stevensanderson.com/2019/08/29/blazor-unit-testing-prototype/)
 * [Bunit - Egil Hansen](https://bunit.egilhansen.com)


## Security contact information

To report a security vulnerability, use the [Tidelift security contact](https://tidelift.com/security). Tidelift will coordinate the fix and disclosure.


## Icon

[Helmet](https://thenounproject.com/term/helmet/9554/) designed by [Leonidas Ikonomou](https://thenounproject.com/alterego) from [The Noun Project](https://thenounproject.com).