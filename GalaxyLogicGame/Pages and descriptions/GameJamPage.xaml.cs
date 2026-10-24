using GalaxyLogicGame.Particles;

namespace GalaxyLogicGame.Pages_and_descriptions;

public partial class GameJamPage : ContentPage, IStarsPage
{
    private StarsParticlesLayout stars;
    public GameJamPage()
	{
        NavigationPage.SetHasNavigationBar(this, false);
        InitializeComponent();
        Functions.ScaleToScreen(this, scaleLayout);
        Functions.FillHeight(scaleLayout);
        SizeChanged += OnDisplaySizeChanged;
    }
    public void AssingStars(StarsParticlesLayout stars)
    {
        this.stars = stars;
        starsLayout.Children.Add(stars);
    }
    private void OnDisplaySizeChanged(object sender, EventArgs args)
    {
        Functions.ScaleToScreen(this, scaleLayout);
        Functions.FillHeight(scaleLayout);
    }
    public async Task TransitionIn()
    {
        await Task.WhenAll(
            this.TranslateToAsync(0, 0, 500, Easing.SinOut),
            this.FadeToAsync(1, 500, Easing.SinOut),
            stars.TransitionUpOut(),
            mainLayout.TranslateToAsync(0, 0, 500, Easing.SinOut),
            mainLayout.FadeToAsync(1, 500));

        starsLayout.Children.Clear();
    }
}