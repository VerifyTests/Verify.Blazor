# Integration testing using Verify.Selenium

[Verify.Selenium](https://github.com/VerifyTests/Verify.Selenium) extends [Verify](https://github.com/VerifyTests/Verify) to allow verification of Web UIs using [Selenium](https://www.selenium.dev/).


## Fixture for shared state

The running instance of the Blazor app and the Selenium driver are expensive to instantiate and should be share between tests. This sample uses xunit, so a [ClassFixture](https://xunit.net/docs/shared-context.html#class-fixture) is used to share state.

snippet: SeleniumFixture


## Tests

snippet: SeleniumUsageTest


### Page results

snippet: SeleniumUsageTest.PageUsage.00.verified.html

[TheTests.PageUsage.01.verified.png](/src/Verify.Blazor.Tests/IntegrationTest/SeleniumUsageTest.PageUsage.01.verified.png):

<img src="/src/Verify.Blazor.Tests/IntegrationTest/SeleniumUsageTest.PageUsage.01.verified.png" width="400px">


### Element results

snippet: SeleniumUsageTest.ElementUsage.00.verified.html

[TheTests.ElementUsage.01.verified.png](/src/Verify.Blazor.Tests/IntegrationTest/SeleniumUsageTest.ElementUsage.01.verified.png):

<img src="/src/Verify.Blazor.Tests/IntegrationTest/SeleniumUsageTest.ElementUsage.01.verified.png">
