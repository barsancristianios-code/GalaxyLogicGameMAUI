using CommunityToolkit.Maui;
using GalaxyLogicGame.Mobile;

namespace GalaxyLogicGameWearOS
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("Rajdhani-Bold.ttf", "TitleFont");
				fonts.AddFont("Rajdhani-SemiBold.ttf", "BodyFont");
				fonts.AddFont("Audiowide-Regular.ttf", "LogoFont");
                })
                .UseMauiCommunityToolkit(); ;

            return builder.Build();
        }
    }
}