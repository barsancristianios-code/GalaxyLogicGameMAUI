namespace GalaxyLogicGame.Services
{
    /// <summary>
    /// Local lifetime statistics. Later phases sync these to the player's
    /// online profile.
    /// </summary>
    public static class PlayerStats
    {
        public static int GamesPlayed => Preferences.Get("stats.gamesPlayed", 0);
        public static int TotalScore => Preferences.Get("stats.totalScore", 0);
        public static int HighestPlanet => Preferences.Get("stats.highestPlanet", 0);
        public static int BestCombo => Preferences.Get("stats.bestCombo", 0);

        public static void RecordGame(int score, int highestPlanet)
        {
            Preferences.Set("stats.gamesPlayed", GamesPlayed + 1);
            Preferences.Set("stats.totalScore", TotalScore + score);
            if (highestPlanet > HighestPlanet) Preferences.Set("stats.highestPlanet", highestPlanet);
        }

        public static void RecordCombo(int combo)
        {
            if (combo > BestCombo) Preferences.Set("stats.bestCombo", combo);
        }
    }
}
