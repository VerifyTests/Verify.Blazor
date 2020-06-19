using System.Threading.Tasks;
using Verify.Blazor.Tests;
using VerifyTests;
using VerifyTests.Blazor;
using VerifyXunit;
using Xunit;

// Non-nullable field is uninitialized
#pragma warning disable CS8618
[UsesVerify]
public class Samples
{
    static Samples()
    {
        #region Enable
        VerifyBlazor.Initialize();
        #endregion
    }

    #region ComponentTest
    [Fact]
    public async Task Component()
    {
        await Verifier.Verify(
            Render.Component<TestComponent>(
                beforeRender: component => { component.Title = "New Title"; }));
    }

    #endregion

}
