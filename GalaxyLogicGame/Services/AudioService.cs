using Plugin.Maui.Audio;

namespace GalaxyLogicGame.Services
{
    /// <summary>
    /// Loads the sound effects and music from the app package (Resources/Raw)
    /// and plays them, respecting the player's settings. Every call is safe to
    /// make before loading finishes or on platforms where audio fails.
    /// </summary>
    public static class AudioService
    {
        public const string Place = "sfx_place.wav";
        public const string Click = "sfx_click.wav";
        public const string GameOver = "sfx_gameover.wav";
        public const string Explosion = "sfx_explosion.wav";
        public const string Music = "music_space.wav";

        private static readonly string[] effects =
        {
            Place, Click, GameOver, Explosion,
            "sfx_merge1.wav", "sfx_merge2.wav", "sfx_merge3.wav", "sfx_merge4.wav", "sfx_merge5.wav",
        };

        private static readonly Dictionary<string, IAudioPlayer> players = new();
        private static IAudioPlayer music;
        private static Task loading;

        /// <summary>Starts loading all sounds once; later calls return the same task.</summary>
        public static Task LoadAsync() => loading ??= LoadInternalAsync();

        private static async Task LoadInternalAsync()
        {
            foreach (string name in effects)
            {
                try
                {
                    Stream stream = await FileSystem.OpenAppPackageFileAsync(name);
                    players[name] = AudioManager.Current.CreatePlayer(stream);
                }
                catch
                {
                    // missing file or unsupported platform: play nothing for this sound
                }
            }

            try
            {
                Stream stream = await FileSystem.OpenAppPackageFileAsync(Music);
                music = AudioManager.Current.CreatePlayer(stream);
                music.Loop = true;
            }
            catch
            {
                music = null;
            }
        }

        public static void Play(string name)
        {
            if (!GameSettings.SoundEnabled) return;
            if (!players.TryGetValue(name, out IAudioPlayer player)) return;

            try
            {
                if (player.IsPlaying) player.Stop();
                player.Volume = GameSettings.SoundVolume;
                player.Play();
            }
            catch
            {
            }
        }

        /// <summary>Merge chime; the pitch rises with the combo (1 to 5).</summary>
        public static void PlayMerge(int combo) => Play($"sfx_merge{Math.Clamp(combo, 1, 5)}.wav");

        public static void StartMusic()
        {
            if (music == null) return;
            ApplyMusicSetting();
        }

        public static void ApplyMusicSetting()
        {
            if (music == null) return;

            try
            {
                music.Volume = GameSettings.MusicVolume;
                if (GameSettings.MusicEnabled && !music.IsPlaying) music.Play();
                else if (!GameSettings.MusicEnabled && music.IsPlaying) music.Pause();
            }
            catch
            {
            }
        }

        public static void PauseMusic()
        {
            try { if (music?.IsPlaying == true) music.Pause(); } catch { }
        }
    }
}
