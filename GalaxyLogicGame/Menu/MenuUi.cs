using GalaxyLogicGame.Services;
using Microsoft.Maui.Controls.Shapes;

namespace GalaxyLogicGame.Menu
{
    /// <summary>Small building blocks shared by the menu pages.</summary>
    public static class MenuUi
    {
        public static readonly Color Accent = Color.FromArgb("45ffed");
        public static readonly Color Panel = Color.FromArgb("1EFFFFFF");
        public static readonly Color PanelStroke = Color.FromArgb("30FFFFFF");
        public static readonly Color MutedText = Color.FromArgb("AAFFFFFF");

        /// <summary>Back arrow and page title.</summary>
        public static Grid CreateHeader(string title, Func<Task> back)
        {
            Grid header = new Grid { HeightRequest = 56, Margin = new Thickness(0, 6, 0, 10) };
            header.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(56)));
            header.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
            header.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(56)));

            Border backButton = CircleButton("‹", 30);
            OnTap(backButton, back);
            header.Add(backButton, 0, 0);

            header.Add(new Label
            {
                Text = title,
                FontFamily = "LogoFont",
                FontSize = 24,
                TextColor = Colors.White,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
            }, 1, 0);
            return header;
        }

        public static Border CircleButton(string text, double fontSize = 22)
        {
            return new Border
            {
                WidthRequest = 44,
                HeightRequest = 44,
                StrokeShape = new Ellipse(),
                Stroke = PanelStroke,
                StrokeThickness = 1,
                BackgroundColor = Panel,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Content = new Label
                {
                    Text = text,
                    FontSize = fontSize,
                    TextColor = Colors.White,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                },
            };
        }

        /// <summary>A rounded translucent card.</summary>
        public static Border Card(View content, double padding = 14)
        {
            return new Border
            {
                StrokeShape = new RoundRectangle { CornerRadius = 18 },
                Stroke = PanelStroke,
                StrokeThickness = 1,
                BackgroundColor = Panel,
                Padding = new Thickness(padding),
                Content = content,
            };
        }

        /// <summary>Wide button: icon, title and an optional line of description.</summary>
        public static Border RowButton(string icon, string title, string description, Func<Task> action)
        {
            Grid grid = new Grid { ColumnSpacing = 12 };
            grid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(40)));
            grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
            grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));

            grid.Add(new Label { Text = icon, FontSize = 28, VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center }, 0, 0);

            VerticalStackLayout texts = new VerticalStackLayout { VerticalOptions = LayoutOptions.Center };
            texts.Add(new Label { Text = title, FontFamily = "TitleFont", FontSize = 22, TextColor = Colors.White });
            if (!string.IsNullOrEmpty(description))
                texts.Add(new Label { Text = description, FontFamily = "BodyFont", FontSize = 14, TextColor = MutedText });
            grid.Add(texts, 1, 0);

            grid.Add(new Label { Text = "›", FontSize = 28, TextColor = MutedText, VerticalOptions = LayoutOptions.Center }, 2, 0);

            Border card = Card(grid, 12);
            OnTap(card, action);
            return card;
        }

        public static Label SectionTitle(string text)
        {
            return new Label
            {
                Text = text.ToUpperInvariant(),
                FontFamily = "TitleFont",
                FontSize = 15,
                CharacterSpacing = 2,
                TextColor = Accent,
                Margin = new Thickness(4, 14, 0, 6),
            };
        }

        /// <summary>Makes a view tappable with a click sound, a vibration and a press animation.</summary>
        public static void OnTap(View view, Func<Task> action)
        {
            bool busy = false;
            TapGestureRecognizer tap = new TapGestureRecognizer();
            tap.Tapped += async (s, e) =>
            {
                if (busy) return;
                busy = true;
                try
                {
                    GameFeedback.ButtonTapped();
                    await view.ScaleToAsync(0.95, 70, Easing.CubicOut);
                    await view.ScaleToAsync(1, 200, Easing.SpringOut);
                    if (action != null) await action();
                }
                finally
                {
                    busy = false;
                }
            };
            view.GestureRecognizers.Add(tap);
        }

        /// <summary>Fades and slides views in one after another.</summary>
        public static async Task StaggerIn(IEnumerable<View> views, int delay = 45, double offsetY = 30)
        {
            List<Task> running = new List<Task>();
            foreach (View view in views)
            {
                view.Opacity = 0;
                view.TranslationY = offsetY;
            }
            foreach (View view in views)
            {
                running.Add(view.FadeToAsync(1, 300, Easing.CubicOut));
                running.Add(view.TranslateToAsync(0, 0, 450, Easing.SpringOut));
                await Task.Delay(delay);
            }
            await Task.WhenAll(running);
        }
    }
}
