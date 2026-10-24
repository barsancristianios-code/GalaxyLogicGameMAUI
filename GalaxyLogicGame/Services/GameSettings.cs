namespace GalaxyLogicGame.Services
{
    /// <summary>
    /// Player-facing settings, persisted with Preferences.
    /// </summary>
    public static class GameSettings
    {
        public static bool MusicEnabled
        {
            get => Preferences.Get("settings.music", true);
            set { Preferences.Set("settings.music", value); AudioService.ApplyMusicSetting(); }
        }

        public static bool SoundEnabled
        {
            get => Preferences.Get("settings.sound", true);
            set => Preferences.Set("settings.sound", value);
        }

        public static bool HapticsEnabled
        {
            get => Preferences.Get("settings.haptics", true);
            set => Preferences.Set("settings.haptics", value);
        }

        public static double MusicVolume
        {
            get => Preferences.Get("settings.musicVolume", 0.5);
            set { Preferences.Set("settings.musicVolume", value); AudioService.ApplyMusicSetting(); }
        }

        public static double SoundVolume
        {
            get => Preferences.Get("settings.soundVolume", 0.8);
            set => Preferences.Set("settings.soundVolume", value);
        }

        public static bool TutorialCompleted
        {
            get => Preferences.Get("tutorialCompleted", false);
            set => Preferences.Set("tutorialCompleted", value);
        }
    }
}
