using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using MarcTron.Plugin;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
//using Android.App;


namespace GalaxyLogicGame.Mobile
{

    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            return new Window(new NavigationPage(new GalaxyLogicGame.Menu.LoadingPage()));
        }

        protected override void OnStart()
        {
            // Handle when your app starts

        }

        protected override void OnSleep()
        {
            GalaxyLogicGame.Services.AudioService.PauseMusic();
        }

        protected override void OnResume()
        {
            GalaxyLogicGame.Services.AudioService.StartMusic();
        }
    }
}