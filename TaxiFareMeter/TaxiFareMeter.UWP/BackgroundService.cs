using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiFareMeter.UWP;
using Xamarin.Forms;

[assembly: Dependency(typeof(BackgroundService))]
namespace TaxiFareMeter.UWP
{
    public class BackgroundService : IBackgroundService
    {
        public string HelloWorld()
        {
            return "Hello, from UWP";
        }
    }
}
