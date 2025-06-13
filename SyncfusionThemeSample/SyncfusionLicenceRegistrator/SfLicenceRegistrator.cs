using Syncfusion.Licensing;

namespace SyncfusionLicenceRegistrator
{
    public static class SfLicenceRegistrator
    {
        public static void IgnoreValidation()
        {
            FusionLicenseProvider.IsBoldLicenseValidation = true;
        }

        public static void Register()
        {
            SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NNaF1cWWhIfEx1RHxQdld5ZFRHallYTnNWUj0eQnxTdEBjXH5XcXRWQWVaVUx2X0lfag==");
        }
    }
}