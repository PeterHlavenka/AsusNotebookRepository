﻿

 using System.Windows;
 using Syncfusion.Licensing;

 namespace SfDataGridRoztahovaniFiltru
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // Registrace Syncfusion licence
            // Pro Community licenci (zdarma) získejte klíč na: https://www.syncfusion.com/products/communitylicense
            // Pokud používáte placenou licenci, nahraďte níže uvedený klíč svým vlastním
            SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1JGaF5cX2FCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdlWX5fcnRWRmBeUkJ+V0dWYEs=");
            
            base.OnStartup(e);
        }
    }
}

