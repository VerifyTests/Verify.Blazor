using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;

#region SeleniumFixture

public class SeleniumFixture :
    IAsyncLifetime
{
    Process? process;
    ChromeDriver? driver;

    public Task InitializeAsync()
    {
        StartBlazorApp();

        StartDriver();

        WaitForRender();
        return Task.CompletedTask;
    }

    void StartBlazorApp()
    {
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory!;
        var binPath = baseDirectory.Replace("Verify.Blazor.Tests", "BlazorApp");
        var projectDir = Path.GetFullPath(Path.Combine(binPath, "../../"));
        ProcessStartInfo startInfo = new("dotnet", "run")
        {
            WorkingDirectory = projectDir
        };
        process = Process.Start(startInfo);
    }

    void StartDriver()
    {
        ChromeOptions options = new();
        options.AddArgument("--no-sandbox");
        options.AddArgument("--headless");
        driver = new(options);
        driver.Manage().Window.Size = new(1024, 768);
        driver.Navigate().GoToUrl("http://localhost:5000");
    }

    void WaitForRender()
    {
        WebDriverWait wait = new(Driver, TimeSpan.FromSeconds(5));
        wait.Until(_ => _.FindElement(By.ClassName("main")));
    }

    public ChromeDriver Driver => driver!;

    public Task DisposeAsync()
    {
        if (driver != null)
        {
            driver.Quit();
            driver.Dispose();
        }

        if (process != null)
        {
            process.Kill();
            process.Dispose();
        }

        return Task.CompletedTask;
    }
}

#endregion