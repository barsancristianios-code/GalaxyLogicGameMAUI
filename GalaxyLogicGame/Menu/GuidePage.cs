using GalaxyLogicGame.Mobile;
using GalaxyLogicGame.Pages_and_descriptions;

namespace GalaxyLogicGame.Menu
{
    /// <summary>Everything about how to play, in one place.</summary>
    public class GuidePage : MenuPageBase
    {
        private readonly List<View> rows = new List<View>();
        private bool introPlayed;

        public GuidePage(MainPage menu) : base("wallpaper3.png")
        {
            VerticalStackLayout stack = new VerticalStackLayout { Padding = new Thickness(18, 14), Spacing = 12 };
            stack.Add(MenuUi.CreateHeader("GUIDE", () => Navigation.PopAsync()));

            rows.Add(MenuUi.RowButton("🎓", "Interactive tutorial", "Play through the basics step by step", menu.StartTutorial));
            rows.Add(MenuUi.RowButton("🌌", "Events", "What each special event does", () => menu.OpenStarsPage(new AboutEventfulGame())));
            rows.Add(MenuUi.RowButton("🪐", "Objects", "Planets, stars, debris and more", () => menu.OpenStarsPage(new TutorialPage(false))));

            VerticalStackLayout basics = new VerticalStackLayout { Spacing = 6 };
            basics.Add(new Label { Text = "THE BASICS", FontFamily = "TitleFont", FontSize = 15, CharacterSpacing = 2, TextColor = MenuUi.Accent });
            basics.Add(Bullet("Tap between planets on the ring to place the new one."));
            basics.Add(Bullet("Merge matching planets to create bigger numbers and score points."));
            basics.Add(Bullet("Several merges in one move make a combo worth extra points."));
            basics.Add(Bullet("If the ring fills up, the game is over."));
            rows.Add(MenuUi.Card(basics));

            foreach (View row in rows) stack.Add(row);
            AddFill(new ScrollView { Content = stack });
        }

        private static Label Bullet(string text)
        {
            return new Label { Text = "•  " + text, FontFamily = "BodyFont", FontSize = 16, TextColor = Colors.White };
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (introPlayed) return;
            introPlayed = true;
            await MenuUi.StaggerIn(rows, 60);
        }
    }
}
