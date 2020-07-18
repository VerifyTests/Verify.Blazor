using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;
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
        var appDll = Path.GetFullPath(Path.Combine(binPath, "BlazorApp.dll"));
        using var process = Process.Start("dotnet", appDll);
        var options = new ChromeOptions();
        options.AddArgument("--no-sandbox");
        options.AddArgument("--headless");
        var driver = new ChromeDriver(options);
        driver.Manage().Window.Size = new Size(1024, 768);
        driver.Navigate().GoToUrl("http://localhost:50419");
        await Verifier.Verify(driver);
        process.Kill();
    }
}