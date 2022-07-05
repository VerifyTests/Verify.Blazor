namespace VerifyTests.Blazor;

public static class BlazorScrubber
{
    public static void ScrubCommentLines() =>
        InnerBlazorScrubber.ScrubCommentLines();
}