using GalaxyLogicGame.Particles;
using Microsoft.Maui.Layouts;

namespace GalaxyLogicGame.Menu
{
    /// <summary>
    /// Base for the menu pages. Like the rest of the game, pages are laid out
    /// on a canvas 360 units wide that is scaled to the screen; the canvas is
    /// as tall as the screen's aspect ratio allows.
    /// </summary>
    public abstract class MenuPageBase : ContentPage
    {
        protected AbsoluteLayout Root { get; }
        protected AbsoluteLayout Canvas { get; }
        protected Image Wallpaper { get; }

        protected MenuPageBase(string wallpaper = "wallpaper1.png")
        {
            NavigationPage.SetHasNavigationBar(this, false);
            BackgroundColor = Colors.Black;

            Root = new AbsoluteLayout();
            Canvas = new AbsoluteLayout();
            AbsoluteLayout.SetLayoutBounds(Canvas, new Rect(0.5, 0.5, 360, 360));
            AbsoluteLayout.SetLayoutFlags(Canvas, AbsoluteLayoutFlags.PositionProportional);

            Wallpaper = new Image { Source = wallpaper, Aspect = Aspect.AspectFill, Opacity = 0.85 };
            AbsoluteLayout.SetLayoutBounds(Wallpaper, new Rect(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(Wallpaper, AbsoluteLayoutFlags.All);
            Canvas.Children.Add(Wallpaper);
            Canvas.Children.Add(new StarsParticlesLayout());

            Root.Children.Add(Canvas);
            Content = Root;

            Rescale();
            SizeChanged += (s, e) => Rescale();
        }

        /// <summary>Height of the canvas in canvas units (width is always 360).</summary>
        protected double CanvasHeight => AbsoluteLayout.GetLayoutBounds(Canvas).Height;

        private void Rescale()
        {
            Functions.ScaleToScreen(this, Canvas);
            Functions.FillHeight(Canvas);
            OnCanvasResized();
        }

        protected virtual void OnCanvasResized() { }

        /// <summary>Adds a view that fills the whole canvas.</summary>
        protected void AddFill(View view)
        {
            AbsoluteLayout.SetLayoutBounds(view, new Rect(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(view, AbsoluteLayoutFlags.All);
            Canvas.Children.Add(view);
        }
    }
}
