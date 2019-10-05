using Prism;
using Prism.Ioc;
using System;
using TaxiFareMeter.ViewModels;
using TaxiFareMeter.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace TaxiFareMeter
{
    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */

        private readonly FareMeterPageViewModel _fareMeterVM;

        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { _fareMeterVM = new FareMeterPageViewModel(); }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/FareMeterPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<FareMeterPage, FareMeterPageViewModel>();
        }

        //protected override void OnStart()
        //{
        //    LoadPersistedValues();
        //}

        //protected override void OnSleep()
        //{
        //    Application.Current.Properties["StatusDisplay"] = _fareMeterVM.StatusDisplay;
        //}

        //protected override void OnResume()
        //{
        //    LoadPersistedValues();
        //}

        //private void LoadPersistedValues()
        //{
        //    if(Application.Current.Properties.ContainsKey("StatusDisplay"))
        //    {
        //        var statusDisplay = (string)Application.Current.Properties["StatusDisplay"];
        //        _fareMeterVM.StatusDisplay = statusDisplay;
        //    }
        //}
    }
}
