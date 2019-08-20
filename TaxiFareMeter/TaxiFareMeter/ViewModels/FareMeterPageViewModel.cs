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

        private string timeDisplay;
        public string TimeDisplay
        {
            get => timeDisplay;
            set => SetProperty(ref timeDisplay, value);
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
        
        public void Timer()
        {
            stopWatch.Start();

            Device.StartTimer(TimeSpan.FromMilliseconds(1000), () =>
            {
                TimeSpan ts = stopWatch.Elapsed;
                TimeDisplay = string.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);

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

        public async void GetSourceLocation()
        {
            var loc1 = await Geolocation.GetLastKnownLocationAsync();

            if (loc1 != null)
            {
                Console.WriteLine($"Source Latitude: {loc1.Latitude}, Source Longitude: {loc1.Longitude}");

                SourceLongitude = loc1.Longitude;
                SourceLatitude = loc1.Latitude;
            }

            //SourceLatitude = 14.578706;
            //SourceLongitude = 121.050499;
            //Console.WriteLine($"Source Latitude: {SourceLatitude}, Source Longitude: {SourceLongitude}");
        }

        public async void GetDestinationLocation()
        {
            var loc2 = await Geolocation.GetLastKnownLocationAsync();

            if (loc2 != null)
            {
                Console.WriteLine($"Destination Latitude: {loc2.Latitude}, Destination Longitude: {loc2.Longitude}");

                DestinationLongitude = loc2.Longitude;
                DestinationLatitude = loc2.Latitude;
            }

            //DestinationLatitude = 14.603078;
            //DestinationLongitude = 121.119543;
            //Console.WriteLine($"Destination Latitude: {DestinationLatitude}, Destination Longitude: {DestinationLongitude}");
        }

        private void ElapsedTime()
        {
            TimeSpan ts = stopWatch.Elapsed;

            DurationRate = Convert.ToInt32(ts.TotalMinutes);
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

        public ICommand ResetCommand => new Command(() =>
        {
            stopWatch.Reset();

            DurationRate = 0;
            DistanceRate = 0;
            TotalFare = 0;

        });

        public ICommand CalculateCommand => new Command(() =>
        {
            GetDistance();

            DistanceRate = Math.Round(DistanceRate * 13.50);
            
            TotalFare = DistanceRate + DurationRate + 40;
        });

        //Function that gets the distance using longitude and latitude
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

                //Added static 3KM
                DistanceRate = dist;

                return dist;
            }
        }

        //Function that converts decimal degrees to radians
        private double DegreesToRadians(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        //Function that converts radians to decimal degrees
        private double RadiansToDegrees(double rad)
        {
            return (rad / Math.PI * 180.0);
        }
    }
}