using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace GalaxyLogicGame.Particles
{

    public partial class StarsParticlesLayout : AbsoluteLayout
    {
        Button[] starsArray = new Button[40];

        public StarsParticlesLayout()
        {
            InitializeComponent();
            InitiateStars();
        }
        private void InitiateStars()
        {
            AbsoluteLayout.SetLayoutBounds(this, new Rect(0, 0, 360, Functions.GetScreenRatio() > 1 ? 360 * Functions.GetScreenRatio() : 360));
            Random random = new Random();
            for (int i = 0; i < starsArray.Length; i++)
            {
                int temp = (int)(720 * Functions.GetScreenRatio());
                starsArray[i] = new Button
                {
                    BackgroundColor = Color.FromArgb("fff"),
                    Opacity = random.NextDouble(),
                    TranslationX = random.Next(0, 360),
                    TranslationY = random.Next(0, (temp > 0) ? temp : 0),
                };

                this.Children.Add(starsArray[i]);
                AbsoluteLayout.SetLayoutBounds(starsArray[i], new Rect(0, 0, 1, 1));
            }
        }
        public async Task TransitionUpIn()
        {
            this.FadeToAsync(1, 500);
            for (int i = 0; i < starsArray.Length; i++)
            {
                starsArray[i].TranslateToAsync(starsArray[i].TranslationX, starsArray[i].TranslationY - 180, 500, Easing.SinIn);
                starsArray[i].ScaleYToAsync(30, 500, Easing.SinIn);
            }
            await Task.Delay(500);
        }
        public async Task TransitionUpOut()
        {
            for (int i = 0; i < starsArray.Length; i++)
            {
                starsArray[i].TranslateToAsync(starsArray[i].TranslationX, starsArray[i].TranslationY - 180, 500, Easing.SinOut);
                starsArray[i].ScaleYToAsync(1, 500, Easing.SinOut);

            }
            await Task.Delay(500);
            this.FadeToAsync(0, 500);
        }
    }
}