using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;

namespace GalaxyLogicGame.Menu
{
    /// <summary>
    /// The animated game logo: a glowing core with numbered planets orbiting
    /// it, and the "GALAXY LOGIC" wordmark underneath.
    /// </summary>
    public class OrbitLogo : AbsoluteLayout
    {
        private readonly AbsoluteLayout ring;
        private readonly AbsoluteLayout innerRing;
        private readonly Border core;
        private readonly List<View> ringPlanets = new();
        private readonly List<View> innerRingPlanets = new();
        private bool spinning;

        public Label Title { get; }
        public Label Subtitle { get; }

        public OrbitLogo(double size = 170)
        {
            WidthRequest = 300;
            HeightRequest = size + 90;

            ring = CreateRing(size, new[] { ("2", "66CDAA"), ("8", "008080"), ("5", "FF2E8B57"), ("10", "FFEE82EE") }, ringPlanets);
            innerRing = CreateRing(size * 0.62, new[] { ("1", "87CEEB"), ("7", "FF4500") }, innerRingPlanets);

            core = new Border
            {
                StrokeShape = new Ellipse(),
                StrokeThickness = 0,
                Background = new RadialGradientBrush(
                    new GradientStopCollection
                    {
                        new GradientStop(Color.FromArgb("FFFFFFFF"), 0),
                        new GradientStop(Color.FromArgb("CCB06BFF"), 0.35f),
                        new GradientStop(Color.FromArgb("006B2BFF"), 1),
                    }),
            };
            AddCentered(core, size * 0.42, size / 2);
            AddCentered(innerRing, size * 0.62, size / 2);
            AddCentered(ring, size, size / 2);

            Title = new Label
            {
                Text = "GALAXY",
                FontFamily = "LogoFont",
                FontSize = 44,
                TextColor = Colors.White,
                CharacterSpacing = 4,
                HorizontalTextAlignment = TextAlignment.Center,
            };
            Subtitle = new Label
            {
                Text = "L O G I C",
                FontFamily = "LogoFont",
                FontSize = 22,
                TextColor = Color.FromArgb("45ffed"),
                HorizontalTextAlignment = TextAlignment.Center,
            };
            AbsoluteLayout.SetLayoutBounds(Title, new Rect(0, size + 2, 300, 52));
            AbsoluteLayout.SetLayoutBounds(Subtitle, new Rect(0, size + 54, 300, 30));
            Children.Add(Title);
            Children.Add(Subtitle);
        }

        private static AbsoluteLayout CreateRing(double size, (string text, string color)[] planets, List<View> created)
        {
            AbsoluteLayout layout = new AbsoluteLayout { WidthRequest = size, HeightRequest = size };

            Border track = new Border
            {
                StrokeShape = new Ellipse(),
                Stroke = Color.FromArgb("26FFFFFF"),
                StrokeThickness = 1,
                BackgroundColor = Colors.Transparent,
            };
            AbsoluteLayout.SetLayoutBounds(track, new Rect(0, 0, size, size));
            layout.Children.Add(track);

            double planetSize = 30;
            for (int i = 0; i < planets.Length; i++)
            {
                double angle = 2 * Math.PI * i / planets.Length;
                double x = size / 2 + Math.Cos(angle) * size / 2 - planetSize / 2;
                double y = size / 2 + Math.Sin(angle) * size / 2 - planetSize / 2;

                Border planet = new Border
                {
                    StrokeShape = new Ellipse(),
                    StrokeThickness = 0,
                    BackgroundColor = Color.FromArgb(planets[i].color),
                    Content = new Label
                    {
                        Text = planets[i].text,
                        FontFamily = "TitleFont",
                        FontSize = 17,
                        TextColor = Functions.DetermineWhiteOrBlack(Color.FromArgb(planets[i].color)),
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                    },
                };
                AbsoluteLayout.SetLayoutBounds(planet, new Rect(x, y, planetSize, planetSize));
                layout.Children.Add(planet);
                created.Add(planet);
            }
            return layout;
        }

        private void AddCentered(View view, double size, double centerY)
        {
            AbsoluteLayout.SetLayoutBounds(view, new Rect(150 - size / 2, centerY - size / 2, size, size));
            Children.Add(view);
        }

        /// <summary>Spins the rings and pulses the core until <see cref="Stop"/> is called.</summary>
        public void Start()
        {
            if (spinning) return;
            spinning = true;
            Spin(ring, ringPlanets, 24000, 1);
            Spin(innerRing, innerRingPlanets, 15000, -1);
            Pulse();
        }

        public void Stop()
        {
            spinning = false;
            ring.CancelAnimations();
            innerRing.CancelAnimations();
            foreach (View planet in ringPlanets.Concat(innerRingPlanets)) planet.CancelAnimations();
            core.CancelAnimations();
        }

        // The planets turn the opposite way so their numbers stay upright.
        private async void Spin(View view, List<View> planets, uint duration, int direction)
        {
            while (spinning)
            {
                view.Rotation = 0;
                foreach (View planet in planets)
                {
                    planet.Rotation = 0;
                    _ = planet.RotateToAsync(-360 * direction, duration, Easing.Linear);
                }
                await view.RotateToAsync(360 * direction, duration, Easing.Linear);
            }
        }

        private async void Pulse()
        {
            while (spinning)
            {
                await core.ScaleToAsync(1.12, 1400, Easing.SinInOut);
                await core.ScaleToAsync(0.95, 1400, Easing.SinInOut);
            }
        }
    }
}
