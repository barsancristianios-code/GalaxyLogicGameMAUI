using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;


namespace GalaxyLogicGame
{

    public partial class ChallengeCompletedPopup : AbsoluteLayout
    {
        public ChallengeCompletedPopup()
        {
            InitializeComponent();
        }

        public async Task Play(AbsoluteLayout layout, string title)
        {
            if (DeviceInfo.Platform != DevicePlatform.Tizen)
            {
                watchShadow.IsVisible = false;
                mobileShadow.IsVisible = true; 
            }
            layout.Children.Add(this);
            this.title.Text = title;

            await mainLayout.FadeToAsync(1, 1000);

            await Task.Delay(500);

            await completedSticker.TranslateToAsync(0, 0, 250, Easing.SpringOut);

            await Task.Delay(2000);

            await mainLayout.TranslateToAsync(-360, 0, 500, Easing.SpringIn);


            layout.Children.Remove(this);
        }
    }
}