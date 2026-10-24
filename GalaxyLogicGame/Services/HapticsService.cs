namespace GalaxyLogicGame.Services
{
    /// <summary>
    /// Short vibrations for game feedback. Does nothing when disabled in
    /// settings or when the device has no vibration motor.
    /// </summary>
    public static class HapticsService
    {
        public static void Tap() => Perform(HapticFeedbackType.Click);

        public static void Merge(int combo)
        {
            if (combo >= 3) Vibrate(35);
            else Perform(HapticFeedbackType.Click);
        }

        public static void Heavy() => Vibrate(120);

        private static void Perform(HapticFeedbackType type)
        {
            if (!GameSettings.HapticsEnabled) return;
            try
            {
                if (HapticFeedback.Default.IsSupported) HapticFeedback.Default.Perform(type);
            }
            catch
            {
            }
        }

        private static void Vibrate(int milliseconds)
        {
            if (!GameSettings.HapticsEnabled) return;
            try
            {
                if (Vibration.Default.IsSupported) Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(milliseconds));
            }
            catch
            {
            }
        }
    }
}
