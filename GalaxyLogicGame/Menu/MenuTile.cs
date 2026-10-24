using GalaxyLogicGame.Services;
using Microsoft.Maui.Controls.Shapes;

namespace GalaxyLogicGame.Menu
{
    /// <summary>
    /// A rounded menu button with an icon and a caption. Tiles for features
    /// that are not built yet show a "SOON" badge.
    /// </summary>
    public class MenuTile : Border
    {
        public event EventHandler Tapped;

        public bool ComingSoon { get; }

        public MenuTile(string icon, string title, bool comingSoon = false)
        {
            ComingSoon = comingSoon;

            StrokeShape = new RoundRectangle { CornerRadius = 18 };
            Stroke = Color.FromArgb("33FFFFFF");
            StrokeThickness = 1;
            Background = new LinearGradientBrush(
                new GradientStopCollection
                {
                    new GradientStop(Color.FromArgb("403B5BFF"), 0),
                    new GradientStop(Color.FromArgb("18FFFFFF"), 1),
                },
                new Point(0, 0), new Point(1, 1));
            Padding = new Thickness(4, 8);
            HeightRequest = 86;

            Grid grid = new Grid { RowSpacing = 2 };
            grid.RowDefinitions.Add(new RowDefinition(GridLength.Star));
            grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));

            grid.Add(new Label
            {
                Text = icon,
                FontSize = 28,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Opacity = comingSoon ? 0.55 : 1,
            }, 0, 0);

            grid.Add(new Label
            {
                Text = title,
                FontFamily = "TitleFont",
                FontSize = 15,
                TextColor = comingSoon ? Color.FromArgb("99FFFFFF") : Colors.White,
                HorizontalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                LineBreakMode = LineBreakMode.TailTruncation,
            }, 0, 1);

            if (comingSoon)
            {
                Border badge = new Border
                {
                    StrokeShape = new RoundRectangle { CornerRadius = 6 },
                    StrokeThickness = 0,
                    BackgroundColor = Color.FromArgb("ff4fd8"),
                    Padding = new Thickness(5, 0),
                    HorizontalOptions = LayoutOptions.End,
                    VerticalOptions = LayoutOptions.Start,
                    Margin = new Thickness(0, -4, 2, 0),
                    Content = new Label { Text = "SOON", FontFamily = "TitleFont", FontSize = 10, TextColor = Colors.White },
                };
                grid.Add(badge, 0, 0);
            }

            Content = grid;

            TapGestureRecognizer tap = new TapGestureRecognizer();
            tap.Tapped += OnTapped;
            GestureRecognizers.Add(tap);
        }

        private async void OnTapped(object sender, TappedEventArgs e)
        {
            GameFeedback.ButtonTapped();
            await this.ScaleToAsync(0.92, 70, Easing.CubicOut);
            await this.ScaleToAsync(1, 220, Easing.SpringOut);
            Tapped?.Invoke(this, EventArgs.Empty);
        }
    }
}
