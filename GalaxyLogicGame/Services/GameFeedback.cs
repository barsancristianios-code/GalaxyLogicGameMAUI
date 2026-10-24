namespace GalaxyLogicGame.Services
{
    /// <summary>
    /// One place for the game code to report moments that deserve sound and
    /// vibration, so the gameplay files only need a single line per moment.
    /// </summary>
    public static class GameFeedback
    {
        public static void PlanetPlaced()
        {
            AudioService.Play(AudioService.Place);
        }

        public static void Merged(int combo)
        {
            AudioService.PlayMerge(combo);
            HapticsService.Merge(combo);
            PlayerStats.RecordCombo(combo);
        }

        public static void Explosion()
        {
            AudioService.Play(AudioService.Explosion);
            HapticsService.Heavy();
        }

        public static void GameOver()
        {
            AudioService.Play(AudioService.GameOver);
            HapticsService.Heavy();
        }

        public static void ButtonTapped()
        {
            AudioService.Play(AudioService.Click);
            HapticsService.Tap();
        }
    }
}
