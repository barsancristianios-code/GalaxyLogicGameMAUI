using GalaxyLogicGame.Mobile;
using GalaxyLogicGame.Services;

namespace GalaxyLogicGame.Menu
{
    /// <summary>Audio, vibration, account and tutorial settings.</summary>
    public class SettingsPage : MenuPageBase
    {
        private readonly List<View> sections = new List<View>();
        private bool introPlayed;

        public SettingsPage(MainPage menu) : base("wallpaper3.png")
        {
            VerticalStackLayout stack = new VerticalStackLayout { Padding = new Thickness(18, 14), Spacing = 4 };
            stack.Add(MenuUi.CreateHeader("SETTINGS", () => Navigation.PopAsync()));

            // Audio
            VerticalStackLayout audio = new VerticalStackLayout { Spacing = 8 };
            audio.Add(SwitchRow("🎵", "Music", GameSettings.MusicEnabled, v => GameSettings.MusicEnabled = v));
            audio.Add(SliderRow(GameSettings.MusicVolume, v => GameSettings.MusicVolume = v));
            audio.Add(SwitchRow("🔊", "Sound effects", GameSettings.SoundEnabled, v => GameSettings.SoundEnabled = v));
            audio.Add(SliderRow(GameSettings.SoundVolume, v => GameSettings.SoundVolume = v));
            AddSection(stack, "Audio", MenuUi.Card(audio));

            // Feedback
            AddSection(stack, "Feedback", MenuUi.Card(SwitchRow("📳", "Vibration", GameSettings.HapticsEnabled, v =>
            {
                GameSettings.HapticsEnabled = v;
                if (v) HapticsService.Tap();
            })));

            // Account
            VerticalStackLayout account = new VerticalStackLayout { Spacing = 4 };
            account.Add(new Label { Text = "Playing as Guest", FontFamily = "TitleFont", FontSize = 20, TextColor = Colors.White });
            account.Add(new Label { Text = "Sign in with Google to save your progress online, add friends and duel. Coming in the next update.", FontFamily = "BodyFont", FontSize = 14, TextColor = MenuUi.MutedText });
            AddSection(stack, "Account", MenuUi.Card(account));

            // Help
            AddSection(stack, "Help", MenuUi.RowButton("🎓", "Replay tutorial", null, menu.StartTutorial));

            // About
            VerticalStackLayout about = new VerticalStackLayout { Spacing = 2 };
            about.Add(new Label { Text = "Galaxy Logic Game", FontFamily = "TitleFont", FontSize = 18, TextColor = Colors.White });
            about.Add(new Label { Text = "Fonts: Rajdhani and Audiowide (SIL Open Font License). Sounds and music generated for this game.", FontFamily = "BodyFont", FontSize = 13, TextColor = MenuUi.MutedText });
            AddSection(stack, "About", MenuUi.Card(about));
            stack.Add(new BoxView { HeightRequest = 30, Color = Colors.Transparent });

            AddFill(new ScrollView { Content = stack });
        }

        private void AddSection(VerticalStackLayout stack, string title, View content)
        {
            VerticalStackLayout section = new VerticalStackLayout();
            section.Add(MenuUi.SectionTitle(title));
            section.Add(content);
            stack.Add(section);
            sections.Add(section);
        }

        private static Grid SwitchRow(string icon, string title, bool value, Action<bool> changed)
        {
            Grid grid = new Grid { ColumnSpacing = 10 };
            grid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(34)));
            grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
            grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
            grid.Add(new Label { Text = icon, FontSize = 22, VerticalOptions = LayoutOptions.Center }, 0, 0);
            grid.Add(new Label { Text = title, FontFamily = "TitleFont", FontSize = 20, TextColor = Colors.White, VerticalOptions = LayoutOptions.Center }, 1, 0);

            Switch toggle = new Switch { IsToggled = value, OnColor = Color.FromArgb("8045ffed"), ThumbColor = MenuUi.Accent, VerticalOptions = LayoutOptions.Center };
            toggle.Toggled += (s, e) => changed(e.Value);
            grid.Add(toggle, 2, 0);
            return grid;
        }

        private static Slider SliderRow(double value, Action<double> changed)
        {
            Slider slider = new Slider
            {
                Minimum = 0,
                Maximum = 1,
                Value = value,
                MinimumTrackColor = MenuUi.Accent,
                MaximumTrackColor = Color.FromArgb("33FFFFFF"),
                ThumbColor = Colors.White,
                Margin = new Thickness(36, 0, 0, 6),
            };
            slider.ValueChanged += (s, e) => changed(e.NewValue);
            return slider;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (introPlayed) return;
            introPlayed = true;
            await MenuUi.StaggerIn(sections, 60);
        }
    }
}
