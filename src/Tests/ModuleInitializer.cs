public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Initialize()
    {
        #region scrubbers

        // remove some noise from the html snapshot
        VerifierSettings.ScrubEmptyLines();
        BlazorScrubber.ScrubCommentLines();
        VerifierSettings.ScrubLinesWithReplace(
            line =>
            {
                var scrubbed = line.Replace("<!--!-->", "");
                if (string.IsNullOrWhiteSpace(scrubbed))
                {
                    return null;
                }

                return scrubbed;
            });
        HtmlPrettyPrint.All();
        VerifierSettings.ScrubLinesContaining("<script src=\"_framework/dotnet.");

        #endregion

        VerifierSettings.UseSsimForPng();

        VerifierSettings.InitializePlugins();
    }
}
