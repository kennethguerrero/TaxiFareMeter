using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using TaxiFareMeter.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(BackgroundService))]
namespace TaxiFareMeter.Droid
{
    [Service]
    public class BackgroundService : Service
    {
        public string HelloWorld()
        {
            return "Hello, from Android";
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            TestBackgroundService();

            return StartCommandResult.NotSticky;
        }

        private Timer timer;
        string tag = "MyLog";

        public void TestBackgroundService()
        {
            timer = new Timer((o) =>
            {
                Log.Debug(tag, "This is a test log from Backgound Service");
            }, null, 0, 2000);
        }      
    }
}