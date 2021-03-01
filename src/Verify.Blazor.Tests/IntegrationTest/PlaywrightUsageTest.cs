using System.Threading.Tasks;
using PlaywrightSharp;
using PlaywrightSharp.Chromium;
using VerifyXunit;
using Xunit;

[UsesVerify]
public class PlaywrightUsageTest :
    IClassFixture<PlaywrightFixture>
{
    IChromiumBrowser browser;

    public PlaywrightUsageTest(PlaywrightFixture fixture)
    {
        browser = fixture.Browser;
    }

    [Fact]
    public async Task PageUsage()
    {
        var page = await browser.NewPageAsync();
        page.ViewportSize.Height = 768;
        page.ViewportSize.Width = 1024;
        await page.GoToAsync("http://localhost:5025");
        await page.WaitForSelectorAsync(".main");
        await Verifier.Verify(page);
    }

    [Fact]
    public async Task ElementUsage()
    {
        var page = await browser.NewPageAsync();
        await page.GoToAsync("http://localhost:5025",LifecycleEvent.DOMContentLoaded);
        await Task.Delay(1000);
        await page.WaitForSelectorAsync(".main");
        var element = await page.QuerySelectorAsync(".content");
        await Verifier.Verify(element);
    }
}