using GalaxyLogicGame.Mobile;
using GalaxyLogicGame.Services;
using Microsoft.Maui.Controls.Shapes;

namespace GalaxyLogicGame.Menu
{
    /// <summary>Player card with lifetime statistics.</summary>
    public class ProfilePage : MenuPageBase
    {
        private readonly List<View> parts = new List<View>();
        private bool introPlayed;

        public ProfilePage() : base("wallpaper2.png")
        {
            VerticalStackLayout stack = new VerticalStackLayout { Padding = new Thickness(18, 14), Spacing = 14 };
            stack.Add(MenuUi.CreateHeader("PROFILE", () => Navigation.PopAsync()));

            VerticalStackLayout player = new VerticalStackLayout { Spacing = 6 };
            player.Add(new Border
            {
                WidthRequest = 84,
                HeightRequest = 84,
                StrokeShape = new Ellipse(),
                Stroke = MenuUi.Accent,
                StrokeThickness = 2,
                BackgroundColor = Color.FromArgb("3B5BFF"),
                HorizontalOptions = LayoutOptions.Center,
                Content = new Label { Text = "?", FontFamily = "LogoFont", FontSize = 38, TextColor = Colors.White, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center },
            });
            player.Add(new Label { Text = "Guest", FontFamily = "LogoFont", FontSize = 24, TextColor = Colors.White, HorizontalOptions = LayoutOptions.Center });
            player.Add(new Label { Text = "Sign in with Google (coming soon) to keep these stats online.", FontFamily = "BodyFont", FontSize = 14, TextColor = MenuUi.MutedText, HorizontalTextAlignment = TextAlignment.Center });
            parts.Add(player);

            Grid stats = new Grid { ColumnSpacing = 10, RowSpacing = 10 };
            stats.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
            stats.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
            for (int i = 0; i < 3; i++) stats.RowDefinitions.Add(new RowDefinition(GridLength.Auto));

            int games = PlayerStats.GamesPlayed;
            stats.Add(Stat("🏆", "Best score", Preferences.Get(MainPage.HIGHSCORE, 0).ToString()), 0, 0);
            stats.Add(Stat("🎮", "Games played", games.ToString()), 1, 0);
            stats.Add(Stat("🪐", "Highest planet", PlayerStats.HighestPlanet.ToString()), 0, 1);
            stats.Add(Stat("🔥", "Best combo", "x" + PlayerStats.BestCombo), 1, 1);
            stats.Add(Stat("✨", "Total score", PlayerStats.TotalScore.ToString()), 0, 2);
            stats.Add(Stat("📈", "Average", games == 0 ? "-" : (PlayerStats.TotalScore / games).ToString()), 1, 2);
            parts.Add(stats);

            foreach (View part in parts) stack.Add(part);
            AddFill(new ScrollView { Content = stack });
        }

        private static Border Stat(string icon, string title, string value)
        {
            VerticalStackLayout content = new VerticalStackLayout { Spacing = 0 };
            content.Add(new Label { Text = icon, FontSize = 22 });
            content.Add(new Label { Text = value, FontFamily = "LogoFont", FontSize = 24, TextColor = Colors.White });
            content.Add(new Label { Text = title, FontFamily = "BodyFont", FontSize = 14, TextColor = MenuUi.MutedText });
            return MenuUi.Card(content, 12);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (introPlayed) return;
            introPlayed = true;
            await MenuUi.StaggerIn(parts, 90);
        }
    }
}
