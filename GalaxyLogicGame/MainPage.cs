using GalaxyLogicGame.Menu;
using GalaxyLogicGame.Pages_and_descriptions;
using GalaxyLogicGame.Particles;
using GalaxyLogicGame.Services;
using GalaxyLogicGame.Tutorial;
using GalaxyLogicGame.Types;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;

namespace GalaxyLogicGame.Mobile
{
    /// <summary>
    /// Main menu: logo, best score, a big Play button and a grid of tiles
    /// for every other part of the game.
    /// </summary>
    public class MainPage : MenuPageBase, IMainPage
    {
        public const string HIGHSCORE = "org.tizen.myApp.highscore";

        public IGameBG gameBG;
        private bool clicked = true;
        private bool introPlayed;

        private readonly Grid layout;
        private readonly Label bestLabel;
        private readonly Border playButton;
        private readonly OrbitLogo logo;
        private readonly List<View> introViews = new List<View>();
        private readonly List<MenuTile> tiles = new List<MenuTile>();

        public MainPage()
        {
            layout = new Grid { Padding = new Thickness(18, 14, 18, 14), RowSpacing = 14 };
            layout.RowDefinitions.Add(new RowDefinition(GridLength.Auto)); // top bar
            layout.RowDefinitions.Add(new RowDefinition(GridLength.Star));
            layout.RowDefinitions.Add(new RowDefinition(GridLength.Auto)); // logo
            layout.RowDefinitions.Add(new RowDefinition(GridLength.Auto)); // best score
            layout.RowDefinitions.Add(new RowDefinition(GridLength.Auto)); // play
            layout.RowDefinitions.Add(new RowDefinition(GridLength.Auto)); // tiles
            layout.RowDefinitions.Add(new RowDefinition(GridLength.Star));
            layout.RowDefinitions.Add(new RowDefinition(GridLength.Auto)); // footer

            // Top bar: account chip and settings
            Grid topBar = new Grid();
            topBar.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
            topBar.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
            Border account = CreateAccountChip();
            MenuUi.OnTap(account, () => Toast.Show(Root, "Google sign-in is coming in the next update!"));
            topBar.Add(account, 0, 0);
            Border settings = MenuUi.CircleButton("⚙");
            MenuUi.OnTap(settings, () => Navigation.PushAsync(new SettingsPage(this)));
            topBar.Add(settings, 1, 0);
            layout.Add(topBar, 0, 0);

            logo = new OrbitLogo(140) { HorizontalOptions = LayoutOptions.Center };
            layout.Add(logo, 0, 2);

            bestLabel = new Label { FontFamily = "TitleFont", FontSize = 20, TextColor = Colors.White, VerticalOptions = LayoutOptions.Center };
            Border best = new Border
            {
                StrokeShape = new RoundRectangle { CornerRadius = 16 },
                Stroke = MenuUi.PanelStroke,
                StrokeThickness = 1,
                BackgroundColor = MenuUi.Panel,
                Padding = new Thickness(16, 4),
                HorizontalOptions = LayoutOptions.Center,
                Content = new HorizontalStackLayout
                {
                    Spacing = 8,
                    Children = { new Label { Text = "🏆", FontSize = 18, VerticalOptions = LayoutOptions.Center }, bestLabel },
                },
            };
            layout.Add(best, 0, 3);

            playButton = CreatePlayButton();
            MenuUi.OnTap(playButton, () => NavigateToGame(new GameBG(), new GameWithEvents()));
            layout.Add(playButton, 0, 4);

            layout.Add(CreateTiles(), 0, 5);

            Label footer = new Label { Text = "Galaxy Logic Game", FontFamily = "BodyFont", FontSize = 12, TextColor = Color.FromArgb("55FFFFFF"), HorizontalOptions = LayoutOptions.Center };
            layout.Add(footer, 0, 7);

            ScrollView scroll = new ScrollView { Content = layout, VerticalScrollBarVisibility = ScrollBarVisibility.Never };
            AddFill(scroll);

            introViews.AddRange(new View[] { topBar, logo, best, playButton });
            UpdateBestLabel();
            OnCanvasResized();
        }

        protected override void OnCanvasResized()
        {
            // let the spacer rows grow on tall screens, scroll on short ones
            if (layout != null) layout.MinimumHeightRequest = CanvasHeight;
        }

        private Border CreateAccountChip()
        {
            Border avatar = new Border
            {
                WidthRequest = 32,
                HeightRequest = 32,
                StrokeShape = new Ellipse(),
                StrokeThickness = 0,
                BackgroundColor = Color.FromArgb("3B5BFF"),
                Content = new Label { Text = "?", FontFamily = "TitleFont", FontSize = 18, TextColor = Colors.White, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center },
            };
            VerticalStackLayout names = new VerticalStackLayout { VerticalOptions = LayoutOptions.Center, Spacing = -2 };
            names.Add(new Label { Text = "Guest", FontFamily = "TitleFont", FontSize = 17, TextColor = Colors.White });
            names.Add(new Label { Text = "Tap to sign in", FontFamily = "BodyFont", FontSize = 12, TextColor = MenuUi.MutedText });

            return new Border
            {
                StrokeShape = new RoundRectangle { CornerRadius = 22 },
                Stroke = MenuUi.PanelStroke,
                StrokeThickness = 1,
                BackgroundColor = MenuUi.Panel,
                Padding = new Thickness(6, 6, 16, 6),
                HorizontalOptions = LayoutOptions.Start,
                Content = new HorizontalStackLayout { Spacing = 10, Children = { avatar, names } },
            };
        }

        private Border CreatePlayButton()
        {
            return new Border
            {
                WidthRequest = 250,
                HeightRequest = 68,
                HorizontalOptions = LayoutOptions.Center,
                StrokeShape = new RoundRectangle { CornerRadius = 34 },
                StrokeThickness = 0,
                Background = new LinearGradientBrush(
                    new GradientStopCollection
                    {
                        new GradientStop(Color.FromArgb("45ffed"), 0),
                        new GradientStop(Color.FromArgb("3b8bff"), 1),
                    },
                    new Point(0, 0), new Point(1, 1)),
                Shadow = new Shadow { Brush = new SolidColorBrush(Color.FromArgb("45ffed")), Radius = 24, Opacity = 0.6f, Offset = new Point(0, 0) },
                Content = new Label
                {
                    Text = "PLAY",
                    FontFamily = "LogoFont",
                    FontSize = 32,
                    CharacterSpacing = 6,
                    TextColor = Color.FromArgb("04121c"),
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                },
            };
        }

        private Grid CreateTiles()
        {
            Grid grid = new Grid { ColumnSpacing = 10, RowSpacing = 10 };
            for (int i = 0; i < 3; i++) grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
            for (int i = 0; i < 3; i++) grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));

            AddTile(grid, 0, 0, "📅", "Daily", null, "Daily challenges are coming soon!");
            AddTile(grid, 1, 0, "⚔️", "Duels", null, "Online duels are coming soon!");
            AddTile(grid, 2, 0, "👥", "Friends", null, "Friends are coming with Google sign-in!");
            AddTile(grid, 0, 1, "🏆", "Ranking", null, "Leaderboards are coming soon!");
            AddTile(grid, 1, 1, "🎯", "Challenges", () => OpenStarsPage(new ChallengesPage()));
            AddTile(grid, 2, 1, "🎮", "Game Jam", () => NavigateToGame(new GameBG(), new GameWithEvents(Gamemode.GameJam)));
            AddTile(grid, 0, 2, "📖", "Guide", () => Navigation.PushAsync(new GuidePage(this)));
            AddTile(grid, 1, 2, "👤", "Profile", () => Navigation.PushAsync(new ProfilePage()));
            AddTile(grid, 2, 2, "🏅", "Awards", null, "Achievements are coming soon!");
            return grid;
        }

        private void AddTile(Grid grid, int column, int row, string icon, string title, Func<Task> action, string soonMessage = null)
        {
            MenuTile tile = new MenuTile(icon, title, action == null);
            tile.Tapped += async (s, e) =>
            {
                if (action != null) await action();
                else await Toast.Show(Root, soonMessage);
            };
            grid.Add(tile, column, row);
            tiles.Add(tile);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            UpdateBestLabel();
            logo.Start();

            if (introPlayed) return;
            introPlayed = true;

            foreach (View view in introViews.Concat(tiles)) view.Opacity = 0;
            await MenuUi.StaggerIn(introViews, 90);
            _ = MenuUi.StaggerIn(tiles.Cast<View>(), 40);
            Breathe();

            if (!GameSettings.TutorialCompleted) await ShowTutorialPrompt();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            logo.Stop();
        }

        private async void Breathe()
        {
            while (true)
            {
                await playButton.ScaleToAsync(1.04, 1100, Easing.SinInOut);
                await playButton.ScaleToAsync(1, 1100, Easing.SinInOut);
            }
        }

        private async Task ShowTutorialPrompt()
        {
            AbsoluteLayout overlay = new AbsoluteLayout { BackgroundColor = Color.FromArgb("B3000000"), Opacity = 0 };
            AbsoluteLayout.SetLayoutBounds(overlay, new Rect(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(overlay, AbsoluteLayoutFlags.All);

            VerticalStackLayout content = new VerticalStackLayout { Spacing = 10 };
            content.Add(new Label { Text = "🚀", FontSize = 44, HorizontalOptions = LayoutOptions.Center });
            content.Add(new Label { Text = "New to Galaxy Logic?", FontFamily = "LogoFont", FontSize = 22, TextColor = Colors.White, HorizontalTextAlignment = TextAlignment.Center });
            content.Add(new Label { Text = "Learn how to place and merge planets in about a minute.", FontFamily = "BodyFont", FontSize = 16, TextColor = MenuUi.MutedText, HorizontalTextAlignment = TextAlignment.Center });

            Border start = CreatePlayButton();
            start.WidthRequest = 230;
            start.HeightRequest = 56;
            ((Label)start.Content).Text = "START";
            ((Label)start.Content).FontSize = 24;
            Label skip = new Label { Text = "Skip for now", FontFamily = "TitleFont", FontSize = 18, TextColor = MenuUi.MutedText, HorizontalOptions = LayoutOptions.Center, Padding = new Thickness(10) };
            content.Add(start);
            content.Add(skip);

            Border card = MenuUi.Card(content, 20);
            card.BackgroundColor = Color.FromArgb("F0121A33");
            card.Scale = 0.8;
            AbsoluteLayout.SetLayoutBounds(card, new Rect(0.5, 0.5, 310, AbsoluteLayout.AutoSize));
            AbsoluteLayout.SetLayoutFlags(card, AbsoluteLayoutFlags.PositionProportional);
            overlay.Children.Add(card);
            Canvas.Children.Add(overlay);

            async Task Close()
            {
                await Task.WhenAll(overlay.FadeToAsync(0, 250), card.ScaleToAsync(0.9, 250));
                Canvas.Children.Remove(overlay);
            }
            MenuUi.OnTap(start, async () => { await Close(); await StartTutorial(); });
            MenuUi.OnTap(skip, async () => { GameSettings.TutorialCompleted = true; await Close(); });

            await Task.WhenAll(overlay.FadeToAsync(1, 250), card.ScaleToAsync(1, 400, Easing.SpringOut));
        }

        public Task StartTutorial() => NavigateToGame(new GameBG(Color.FromArgb("222")), new TutorialPlacingPlanets());

        /// <summary>Opens one of the original pages that animate in with the star streaks.</summary>
        public async Task OpenStarsPage(IStarsPage page)
        {
            StarsParticlesLayout stars = new StarsParticlesLayout();
            page.AssingStars(stars);
            await Navigation.PushAsync((Page)page, false);
            await page.TransitionIn();
        }

        private async Task NavigateToGame(GameBG gameBG, CasualGame game)
        {
            if (!clicked) return;
            clicked = false;

            this.gameBG?.StopLoop();
            this.gameBG = gameBG;
            gameBG.StartLoop();

            game.BG = gameBG;
            game.MainMenuPage = this;

            await Navigation.PushAsync(gameBG, false);
            await gameBG.Setup(game);

            clicked = true;
        }

        private void UpdateBestLabel()
        {
            bestLabel.Text = Preferences.Get("experimental", false) ? "EXPERIMENTAL MODE" : "BEST  " + Highscore;
        }

        public int Highscore
        {
            get => Preferences.Get(HIGHSCORE, 0);
            set { Preferences.Set(HIGHSCORE, value); UpdateBestLabel(); }
        }

        public string HighscoreLabel { get => bestLabel.Text; set => bestLabel.Text = value; }

        public bool IsUltra => true;
    }
}
