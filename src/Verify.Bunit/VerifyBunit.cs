using Bunit;

namespace VerifyTests
{
    public static class VerifyBunit
    {
        public static void Initialize()
        {
            VerifierSettings.RegisterFileConverter<IRenderedFragment>("html", FragmentToStream.Convert);
            VerifierSettings.ModifySerialization(settings =>
            {
                settings.AddExtraSettings(serializerSettings =>
                {
                    var converters = serializerSettings.Converters;
                    converters.Add(new RenderedFragmentConverter());
                });
            });
        }
    }
}