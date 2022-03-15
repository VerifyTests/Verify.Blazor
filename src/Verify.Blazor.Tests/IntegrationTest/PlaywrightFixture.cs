using Microsoft.Playwright;

public class PlaywrightFixture :
    IAsyncLifetime
{
    IPlaywright? playwright;
    Process? process;
    IBrowser? browser;

    public IBrowser Browser => browser!;

    public async Task InitializeAsync()
    {
        StartBlazorApp();
        playwright = await Playwright.CreateAsync();
        browser = await playwright.Chromium.LaunchAsync();
    }

    void StartBlazorApp()
    {
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var binPath = baseDirectory.Replace("Verify.Blazor.Tests", "BlazorApp");
        var projectDir = Path.GetFullPath(Path.Combine(binPath, "../../"));
        var startInfo = new ProcessStartInfo("dotnet", "run")
        {
            WorkingDirectory = projectDir
        };
        process = Process.Start(startInfo);
    }

    public async Task DisposeAsync()
    {
        if (browser != null)
        {
            await browser.DisposeAsync();
        }

        playwright?.Dispose();

        if (process != null)
        {
            process.Kill();
            process.Dispose();
        }
    }
}