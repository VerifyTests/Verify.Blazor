public static class ModuleInitializer
{
    #region BunitEnableExcludeComponent

    [ModuleInitializer]
    public static void Initialize() =>
        VerifyBunit.Initialize(excludeComponent: true);

    #endregion

    [ModuleInitializer]
    public static void InitializeOther() =>
        VerifyDiffPlex.Initialize();
}