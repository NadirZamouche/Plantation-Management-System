using PlantProMobileApp.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlantProMobileApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            if (CurrentPropertiesService.IsAuth())
            {
                MainPage = new NavigationPage(new MainPage());
            }
            else
            {
                MainPage = new NavigationPage(new LoginPage());
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
