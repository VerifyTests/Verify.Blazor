static class InnerBlazorScrubber
{
    public static void ScrubCommentLines() =>
        VerifierSettings.ScrubLinesWithReplace(_ =>
        {
            var scrubbed = _.Replace("<!--!-->", "");
            if (string.IsNullOrWhiteSpace(scrubbed))
            {
                return null;
            }

            return scrubbed;
        });
}