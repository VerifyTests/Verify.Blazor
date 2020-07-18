using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using VerifyTests;

#region SeleniumFixture

public class SeleniumFixture :
    IDisposable
{
    Process? process;
    ChromeDriver? driver;

    public SeleniumFixture()
    {
        // remove some noise from the html snapshot
        VerifierSettings.ScrubLinesContaining("<!--!-->");

        VerifySelenium.Enable();

        StartBlazorApp();

        StartDriver();

        WaitForRender();
    }

    void StartBlazorApp()
    {
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory!;
        var binPath = baseDirectory.Replace("Verify.Blazor.Tests", "BlazorApp");
        var projectDir = Path.GetFullPath(Path.Combine(binPath, "../../"));
        var startInfo = new ProcessStartInfo("dotnet", "run")
        {
            WorkingDirectory = projectDir
        };
        process = Process.Start(startInfo);
    }

    void StartDriver()
    {
        var options = new ChromeOptions();
        options.AddArgument("--no-sandbox");
        options.AddArgument("--headless");
        driver = new ChromeDriver(options);
        driver.Manage().Window.Size = new Size(1024, 768);
        driver.Navigate().GoToUrl("http://localhost:5000");
    }

    void WaitForRender()
    {
        var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(5));
        wait.Until(drv => drv.FindElement(By.ClassName("main")));
    }

    public ChromeDriver Driver => driver!;

    public void Dispose()
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
    }
}

#endregion