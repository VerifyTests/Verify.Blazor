#region BunitEnable

public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Initialize()
    {
        VerifyBunit.Initialize();
    }
}

#endregion