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

                //mod divisble by 10
                if(tickControl % 10 == 0)
                {
                    GetLocation();
                }

                return true;
            });
        }

        private static async void GetLocation()
        {
            var loc1 = await Geolocation.GetLastKnownLocationAsync();

            if (loc1 != null)
            {
                //Console.WriteLine($"Latitude: {loc1.Latitude}, Longitude: {loc1.Longitude}");
                await Application.Current.MainPage.DisplayAlert("Test", loc1.Latitude.ToString() + "\n" + loc1.Longitude.ToString(), "OK");
            }
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
            distanceRate = Math.Round(distanceRate * 13.50);
            
            TotalFare = DistanceRate + DurationRate + 40;
        });
	}
}