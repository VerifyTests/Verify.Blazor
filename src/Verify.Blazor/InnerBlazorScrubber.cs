static class InnerBlazorScrubber
{
    public static void ScrubCommentLines() =>
        VerifierSettings.ScrubLinesWithReplace(s =>
        {
            var scrubbed = s.Replace("<!--!-->", "");
            if (string.IsNullOrWhiteSpace(scrubbed))
            {
                return null;
            }

            return scrubbed;
        });
}