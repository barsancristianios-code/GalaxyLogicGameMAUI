using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;

namespace GalaxyLogicGame.Menu
{
    /// <summary>A short message that slides up at the bottom of a page and fades away.</summary>
    public static class Toast
    {
        public static async Task Show(AbsoluteLayout host, string text, int milliseconds = 1800)
        {
            Border toast = new Border
            {
                StrokeShape = new RoundRectangle { CornerRadius = 16 },
                Stroke = Color.FromArgb("5545ffed"),
                StrokeThickness = 1,
                BackgroundColor = Color.FromArgb("E6101830"),
                Padding = new Thickness(16, 8),
                Opacity = 0,
                TranslationY = 20,
                InputTransparent = true,
                Content = new Label
                {
                    Text = text,
                    FontFamily = "BodyFont",
                    FontSize = 16,
                    TextColor = Colors.White,
                    HorizontalTextAlignment = TextAlignment.Center,
                },
            };
            AbsoluteLayout.SetLayoutBounds(toast, new Rect(0.5, 0.94, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            AbsoluteLayout.SetLayoutFlags(toast, AbsoluteLayoutFlags.PositionProportional);
            host.Children.Add(toast);

            await Task.WhenAll(toast.FadeToAsync(1, 200), toast.TranslateToAsync(0, 0, 250, Easing.CubicOut));
            await Task.Delay(milliseconds);
            await toast.FadeToAsync(0, 300);
            host.Children.Remove(toast);
        }
    }
}
