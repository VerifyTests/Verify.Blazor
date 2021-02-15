#if DEBUG

using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using VerifyXunit;
using Xunit;

#region SeleniumUsageTest

[UsesVerify]
public class SeleniumUsageTest :
    IClassFixture<SeleniumFixture>
{
    RemoteWebDriver driver;

    public SeleniumUsageTest(SeleniumFixture fixture)
    {
        driver = fixture.Driver;
    }

    [Fact]
    public async Task PageUsage()
    {
        await Verifier.Verify(driver);
    }

    [Fact]
    public async Task ElementUsage()
    {
        var element = driver.FindElement(By.ClassName("content"));
        await Verifier.Verify(element);
    }
}

#endregion

#endif