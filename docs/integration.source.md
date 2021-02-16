# Integration testing using Verify.Playwright

[Verify.Playwright](https://github.com/VerifyTests/Verify.HeadlessBrowsers#playwright-usage) extends [Verify](https://github.com/VerifyTests/Verify) to allow verification of Web UIs using [Playwright for .NET](https://github.com/microsoft/playwright-sharp/).

This sample shows how to use Verify.Playwright to perform snapshot testing (html and image) of a running Blazor app.


## Fixture for shared state

The running instance of the Blazor app and the Playwright driver are expensive to instantiate and should be share between tests. This sample uses xunit, so a [ClassFixture](https://xunit.net/docs/shared-context.html#class-fixture) is used to share state.

snippet: PlaywrightFixture.cs


## Tests

Test can now verify the page state or the state of a specific element.

snippet: PlaywrightUsageTest.cs


### Page results

snippet: PlaywrightUsageTest.PageUsage.00.verified.html

[PlaywrightUsageTest.PageUsage.01.verified.png](/src/Verify.Blazor.Tests/IntegrationTest/PlaywrightUsageTest.PageUsage.01.verified.png):

<img src="/src/Verify.Blazor.Tests/IntegrationTest/PlaywrightUsageTest.PageUsage.01.verified.png" width="400px">


### Element results

snippet: PlaywrightUsageTest.ElementUsage.00.verified.html

[PlaywrightUsageTest.ElementUsage.01.verified.png](/src/Verify.Blazor.Tests/IntegrationTest/PlaywrightUsageTest.ElementUsage.01.verified.png):

<img src="/src/Verify.Blazor.Tests/IntegrationTest/PlaywrightUsageTest.ElementUsage.01.verified.png" width="400px">
