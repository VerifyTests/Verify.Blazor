using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using VerifyTests;
using VerifyXunit;
using Xunit;

[UsesVerify]
public class Integration
{
    [Fact]
    public async Task Component()
    {
        VerifySelenium.Enable();

        var binPath = AppDomain.CurrentDomain.BaseDirectory!.Replace("Verify.Blazor.Tests", "BlazorApp");
        var projectDirectory = Path.GetFullPath(Path.Combine(binPath, "../../"));
        var startInfo = new ProcessStartInfo("dotnet", "run")
        {
            WorkingDirectory = projectDirectory
        };
        using var process = Process.Start(startInfo);
        var options = new ChromeOptions();
        options.AddArgument("--no-sandbox");
        options.AddArgument("--headless");
        var driver = new ChromeDriver(options);
        driver.Manage().Window.Size = new Size(1024, 768);
        driver.Navigate().GoToUrl("http://localhost:5000");

        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        wait.Until(drv => drv.FindElement(By.ClassName("main")));

        await Verifier.Verify(driver);
        process.Kill();
    }
}