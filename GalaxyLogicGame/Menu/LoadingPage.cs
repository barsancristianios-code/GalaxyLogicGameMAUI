using GalaxyLogicGame.Mobile;
using GalaxyLogicGame.Services;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;

namespace GalaxyLogicGame.Menu
{
    /// <summary>
    /// First screen: animated logo and a progress bar while sounds load,
    /// then a smooth hand-off to the main menu.
    /// </summary>
    public class LoadingPage : MenuPageBase
    {
        private static readonly string[] tips =
        {
            "Tip: Chain merges in one move for big combo bonuses.",
            "Tip: Keep an eye on the dots under your score - the ring is filling up!",
            "Tip: Events change the rules for a few turns. Read them in the Guide.",
            "Tip: Turn music and vibration on or off in Settings.",
        };

        private readonly OrbitLogo logo;
        private readonly BoxView progressFill;
        private readonly Label status;
        private readonly Label tip;
        private bool started;

        public LoadingPage()
        {
            logo = new OrbitLogo(180);
            AbsoluteLayout.SetLayoutBounds(logo, new Rect(0.5, 0.36, 300, AbsoluteLayout.AutoSize));
            AbsoluteLayout.SetLayoutFlags(logo, AbsoluteLayoutFlags.PositionProportional);
            Canvas.Children.Add(logo);

            AbsoluteLayout track = new AbsoluteLayout { WidthRequest = 220, HeightRequest = 6 };
            track.Children.Add(new BoxView { Color = Color.FromArgb("26FFFFFF"), CornerRadius = 3, WidthRequest = 220, HeightRequest = 6 });
            progressFill = new BoxView { Color = MenuUi.Accent, CornerRadius = 3, WidthRequest = 220, HeightRequest = 6, AnchorX = 0, ScaleX = 0 };
            track.Children.Add(progressFill);
            AbsoluteLayout.SetLayoutBounds(track, new Rect(0.5, 0.78, 220, 6));
            AbsoluteLayout.SetLayoutFlags(track, AbsoluteLayoutFlags.PositionProportional);
            Canvas.Children.Add(track);

            status = new Label { Text = "Preparing the galaxy...", FontFamily = "BodyFont", FontSize = 15, TextColor = MenuUi.MutedText, HorizontalTextAlignment = TextAlignment.Center };
            AbsoluteLayout.SetLayoutBounds(status, new Rect(0.5, 0.815, 300, AbsoluteLayout.AutoSize));
            AbsoluteLayout.SetLayoutFlags(status, AbsoluteLayoutFlags.PositionProportional);
            Canvas.Children.Add(status);

            tip = new Label { Text = tips[Random.Shared.Next(tips.Length)], FontFamily = "BodyFont", FontSize = 14, TextColor = Color.FromArgb("88FFFFFF"), HorizontalTextAlignment = TextAlignment.Center, Opacity = 0 };
            AbsoluteLayout.SetLayoutBounds(tip, new Rect(0.5, 0.93, 300, AbsoluteLayout.AutoSize));
            AbsoluteLayout.SetLayoutFlags(tip, AbsoluteLayoutFlags.PositionProportional);
            Canvas.Children.Add(tip);

            logo.Opacity = 0;
            logo.Scale = 0.7;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (started) return;
            started = true;

            logo.Start();
            _ = tip.FadeToAsync(1, 800);
            await Task.WhenAll(
                logo.FadeToAsync(1, 600, Easing.CubicOut),
                logo.ScaleToAsync(1, 900, Easing.SpringOut));

            Task loading = AudioService.LoadAsync();
            _ = progressFill.ScaleXToAsync(0.7, 900, Easing.CubicOut);
            status.Text = "Tuning the stars...";
            await Task.WhenAll(loading, Task.Delay(1000));

            await progressFill.ScaleXToAsync(1, 250, Easing.CubicOut);
            status.Text = "Ready!";
            AudioService.StartMusic();
            await Task.Delay(250);

            await Task.WhenAll(
                logo.ScaleToAsync(1.25, 400, Easing.CubicIn),
                Root.FadeToAsync(0, 400, Easing.CubicIn));
            logo.Stop();

            MainPage menu = new MainPage();
            Navigation.InsertPageBefore(menu, this);
            await Navigation.PopAsync(false);
        }
    }
}
