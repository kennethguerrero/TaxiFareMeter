using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TaxiFareMeter.ViewModels
{
	public class FareMeterPageViewModel : BindableBase
	{
        private double durationRate;
        public double DurationRate
        {
            get => durationRate;
            set => SetProperty(ref durationRate, value);
        }

        private double distanceRate;
        public double DistanceRate
        {
            get => distanceRate;
            set => SetProperty(ref distanceRate, value);
        }

        private double totalFare;
        public double TotalFare
        {
            get => totalFare;
            set => SetProperty(ref totalFare, value);
        }

        private string statusDisplay;
        public string StatusDisplay
        {
            get => statusDisplay;
            set => SetProperty(ref statusDisplay, value);
        }

        private double sourceLongitude;
        public double SourceLongitude
        {
            get => sourceLongitude;
            set => SetProperty(ref sourceLongitude, value);
        }

        private double sourceLatitude;
        public double SourceLatitude
        {
            get => sourceLatitude;
            set => SetProperty(ref sourceLatitude, value);
        }

        private double destinationLongitude;
        public double DestinationLongitude
        {
            get => destinationLongitude;
            set => SetProperty(ref destinationLongitude, value);
        }

        private double destinationLatitude;
        public double DestinationLatitude
        {
            get => destinationLatitude;
            set => SetProperty(ref destinationLatitude, value);
        }

        Stopwatch stopWatch;

        private int tickControl = 0;

        public FareMeterPageViewModel()
        {
            stopWatch = new Stopwatch();
        }

        public ICommand StartCommand => new Command(() =>
        {
            Timer();
        });

        public ICommand StopCommand => new Command(() =>
        {
            stopWatch.Stop();
            ElapsedTime();
        });

        public ICommand CalculateCommand => new Command(() =>
        {
            GetDistance();

            DistanceRate = Math.Round(DistanceRate * 13.50);

            TotalFare = DistanceRate + DurationRate + 40;
        });

        public ICommand ResetCommand => new Command(() =>
        {
            stopWatch.Reset();

            DurationRate = 0;
            DistanceRate = 0;
            TotalFare = 0;

            StatusDisplay = "Travel Pending";

        });

        public void Timer()
        {
            Application.Current.Properties["TimeStarted"] = DateTime.Now;

            stopWatch.Start();

            StatusDisplay = "Travel Started";

            Device.StartTimer(TimeSpan.FromMilliseconds(1000), () =>
            {
                var startTime = (DateTime)Application.Current.Properties["TimeStarted"];
                var endTime = DateTime.Now.Subtract(startTime);

                //TimeSpan ts = stopWatch.Elapsed;
                //TimeDisplay = string.Format("{0:00}:{1:00}:{2:00}", endTime.Hours, endTime.Minutes, endTime.Seconds);

                Console.WriteLine($"Start Time: {startTime}, End Time {endTime}");

                tickControl++;

                if(tickControl <= 1)
                {
                    GetSourceLocation();
                }
                //mod divisble by 10
                else if(tickControl % 10 == 0)
                {
                    GetDestinationLocation();
                }

                return true;
            });
        }

        private void ElapsedTime()
        {
            TimeSpan endTime = stopWatch.Elapsed;

            DurationRate = Convert.ToInt32(endTime.TotalMinutes);

            StatusDisplay = "Travel Ended";
        }

        public async void GetSourceLocation()
        {
            var loc1 = await Geolocation.GetLastKnownLocationAsync();

            if (loc1 != null)
            {
                SourceLongitude = loc1.Longitude;
                SourceLatitude = loc1.Latitude;
            }

            //For testing purposes
            //SourceLatitude = 14.578706;
            //SourceLongitude = 121.050499;
            //Console.WriteLine($"Source Latitude: {SourceLatitude}, Source Longitude: {SourceLongitude}");
        }

        public async void GetDestinationLocation()
        {
            var loc2 = await Geolocation.GetLastKnownLocationAsync();

            if (loc2 != null)
            {
                DestinationLongitude = loc2.Longitude;
                DestinationLatitude = loc2.Latitude;
            }

            //For testing purposes
            //DestinationLatitude = 14.603078;
            //DestinationLongitude = 121.119543;
            //Console.WriteLine($"Destination Latitude: {DestinationLatitude}, Destination Longitude: {DestinationLongitude}");
        }

        //Gets the distance using longitude and latitude
        private double GetDistance()
        {
            if ((SourceLongitude == DestinationLongitude) && (SourceLatitude == DestinationLatitude))
            {
                return 0;
            }
            else
            {
                double theta = SourceLongitude - DestinationLongitude;
                double dist = Math.Sin(DegreesToRadians(SourceLatitude)) * Math.Sin(DegreesToRadians(DestinationLatitude)) + Math.Cos(DegreesToRadians(SourceLatitude))
                    * Math.Cos(DegreesToRadians(DestinationLatitude)) * Math.Cos(DegreesToRadians(theta));
                dist = Math.Acos(dist);
                dist = RadiansToDegrees(dist);
                dist = dist * 60 * 1.1515;

                //Conversion of distance to KM (defualt Miles)
                dist = dist * 1.609344;

                DistanceRate = dist;

                return dist;
            }
        }

        //Converts decimal degrees to radians
        private double DegreesToRadians(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        //Converts radians to decimal degrees
        private double RadiansToDegrees(double rad)
        {
            return (rad / Math.PI * 180.0);
        }

        public ICommand AlertCommand => new Command(() =>
        {
            IBackgroundService service = DependencyService.Get<IBackgroundService>();

            Application.Current.MainPage.DisplayAlert("TEST", service.HelloWorld(), "OK");
        });
    }
}