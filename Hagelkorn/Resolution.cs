namespace Hagelkorn
{
    /// <summary>
    /// Helper type for specifying minimum time resolutions.
    /// </summary>
    public static class Resolution
    {
        /// <summary>
        /// Length of a second in seconds.
        /// </summary>
        public const double Seconds = 1;

        /// <summary>
        /// Length of a microsecond in seconds.
        /// </summary>
        public const double Microseconds = 1e-6;

        /// <summary>
        /// Length of a millisecond in seconds.
        /// </summary>
        public const double Milliseconds = 1e-3;

        /// <summary>
        /// Length of a minute in seconds.
        /// </summary>
        public const double Minutes = 60;

        /// <summary>
        /// Length of a hour in seconds.
        /// </summary>
        public const double Hours = 3600;

        /// <summary>
        /// Length of a day in seconds.
        /// </summary>
        public const double Days = 86400;
    }
}
