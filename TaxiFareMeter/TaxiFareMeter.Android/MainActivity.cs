using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using AndroidX.Work;
using Prism;
using Prism.Ioc;
using System;
using System.Threading.Tasks;
using TaxiFareMeter.Droid;
using Xamarin.Forms;

namespace TaxiFareMeter.Droid
{
    [Activity(Label = "Taxi Fare", Icon = "@mipmap/taxi", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            var intent = new Intent(this, typeof(BackgroundService));
            StartService(intent);

            //if(Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            //{
            //    StartForegroundService(intent);
            //}
            //else
            //{
            //    StartService(intent);
            //}

            Xamarin.Essentials.Platform.Init(this, bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App(new AndroidInitializer()));

            //PeriodicWorkRequest periodicWorkRequest = PeriodicWorkRequest.Builder.From<LocationWorker>(TimeSpan.FromMinutes(1)).Build();

            //WorkManager.Instance.Enqueue(periodicWorkRequest);
            
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
        }
    }
}

