using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using VerifyTests;
using VerifyXunit;
using Xunit;

#region SeleniumUsageTest

[UsesVerify]
public class SeleniumUsageTest :
    IClassFixture<SeleniumFixture>
{
    RemoteWebDriver driver;
    VerifySettings settings;

    public SeleniumUsageTest(SeleniumFixture fixture)
    {
        settings = new VerifySettings();
        settings.AutoVerify();
        driver = fixture.Driver;
    }

    [Fact]
    public async Task PageUsage()
    {
        await Verifier.Verify(driver, settings);
    }

    [Fact]
    public async Task ElementUsage()
    {
        var element = driver.FindElement(By.ClassName("content"));
        await Verifier.Verify(element, settings);
    }
}

#endregion