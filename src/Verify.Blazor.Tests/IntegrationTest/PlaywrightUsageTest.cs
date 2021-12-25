using Microsoft.Playwright;

[UsesVerify]
public class PlaywrightUsageTest :
    IClassFixture<PlaywrightFixture>
{
    IBrowser browser;

    public PlaywrightUsageTest(PlaywrightFixture fixture)
    {
        browser = fixture.Browser;
    }

    [Fact]
    public async Task PageUsage()
    {
        var page = await browser.NewPageAsync();
        var size = page.ViewportSize!;
        size.Height = 768;
        size.Width = 1024;
        await page.GotoAsync("http://localhost:5025");
        await page.WaitForSelectorAsync(".main");
        await Verify(page);
    }

    [Fact]
    public async Task ElementUsage()
    {
        var page = await browser.NewPageAsync();
        await page.GotoAsync("http://localhost:5025");
        await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        await page.WaitForSelectorAsync(".main");
        var element = await page.QuerySelectorAsync(".content");
        await Verify(element);
    }
}