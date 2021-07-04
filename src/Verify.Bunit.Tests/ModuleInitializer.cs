using System.Runtime.CompilerServices;
using VerifyTests;
using VerifyXunit;

#region BunitEnable

[UsesVerify]
public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Initialize()
    {
        VerifyBunit.Initialize();
    }
}

#endregion