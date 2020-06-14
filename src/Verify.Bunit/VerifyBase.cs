
namespace VerifyBunit
{
    [ObsoleteEx(
        Message = @"Verify.Bunit no longer requires a base class. Instead:
 * Add a nuget reference to `Verify.Xunit`.
 * Add a [UsesVerifyAttribute]` to any class that uses Verify.
 * Use the static `Verifier.Verify()`.",
        TreatAsErrorFromVersion = "5.0")]
    public class VerifyBase
    {
    }
}