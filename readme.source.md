# <img src="/src/icon.png" height="30px"> Verify.Bunit

[![Build status](https://ci.appveyor.com/api/projects/status/18lflc71pchw565r?svg=true)](https://ci.appveyor.com/project/SimonCropp/Verify-Bunit)
[![NuGet Status](https://img.shields.io/nuget/v/Verify.Bunit.svg)](https://www.nuget.org/packages/Verify.Bunit/)

Support for rendering a [Blazor Component](https://docs.microsoft.com/en-us/aspnet/core/blazor/#components) to a verified file via [bunit](https://bunit.egilhansen.com).

Support is available via a [Tidelift Subscription](https://tidelift.com/subscription/pkg/nuget-verify.bunit?utm_source=nuget-verify.bunit&utm_medium=referral&utm_campaign=enterprise).

toc


## NuGet package

https://nuget.org/packages/Verify.Bunit/


## Usage

Enable at startup:

snippet: Enable

Given the following Component:

snippet: TestComponent.razor

This test:

snippet: ComponentTest

Will produce:

The component rendered as html `...Component.verified.html`:

snippet: Samples.Component.verified.html

And the current model rendered as txt `...Component.info.verified.txt`:

snippet: Samples.Component.info.verified.txt


## Security contact information

To report a security vulnerability, use the [Tidelift security contact](https://tidelift.com/security). Tidelift will coordinate the fix and disclosure.


## Icon

[Helmet](https://thenounproject.com/term/helmet/9554/) designed by [Leonidas Ikonomou](https://thenounproject.com/alterego) from [The Noun Project](https://thenounproject.com).