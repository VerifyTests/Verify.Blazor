using ImageMagick;
using VerifyTests.AngleSharp;
using VerifyTests.Blazor;

public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Initialize()
    {
        #region scrubbers

        // remove some noise from the html snapshot
        VerifierSettings.ScrubEmptyLines();
        BlazorScrubber.ScrubCommentLines();
        VerifierSettings.ScrubLinesWithReplace(s =>
        {
            var scrubbed = s.Replace("<!--!-->", "");
            if (string.IsNullOrWhiteSpace(scrubbed))
            {
                return null;
            }

            return scrubbed;
        });
        HtmlPrettyPrint.All();
        VerifierSettings.ScrubLinesContaining("<script src=\"_framework/dotnet.");

        #endregion

        VerifyImageMagick.RegisterComparers(
            threshold: .01,
            metric: ErrorMetric.MeanAbsolute);

        VerifierSettings.InitializePlugins();
    }
}