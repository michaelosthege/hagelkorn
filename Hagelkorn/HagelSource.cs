using System;
using System.Collections.Generic;

namespace Hagelkorn
{
    /// <summary>
    /// An ID-generator that exposes some internal parameters.
    /// </summary>
    public class HagelSource
    {
        public const string DEFAULT_ALPHABET = "13456789ABCDEFHKLMNPQRTWXYZ";
        public readonly DateTime DEFAULT_START = new DateTime(2018, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static Random rnd = new Random();

        public string Alphabet { get; private set; }
        public int B { get { return Alphabet.Length; } }
        public DateTime Start { get; private set; }
        public double TotalSeconds { get; private set; }
        public double OverflowYears { get; private set; }
        public DateTime End { get; private set; }
        public int Digits { get; private set; }
        public long Combinations { get; private set; }
        public double Resolution { get; private set; }

        /// <summary>
        /// Creates an ID-generator that is slightly faster and a bit more transparent.
        /// </summary>
        /// <param name="resolution">maximum duration in seconds for an increment in the id</param>
        /// <param name="alphabet">the (sorted) characters to be used in the ID generation</param>
        /// <param name="start">beginning of timeline</param>
        /// <param name="overflow_years">number of years after which the key length will increase by 1</param>
        public HagelSource(double resolution=Hagelkorn.Resolution.Seconds, string alphabet = DEFAULT_ALPHABET, DateTime? start=null, double overflow_years=10)
        {
            if (start == null)
                start = DEFAULT_START;
            Alphabet = alphabet;
            Start = ((DateTime)start).ToUniversalTime();
            TotalSeconds = overflow_years * 31536000;
            End = Start.AddSeconds(TotalSeconds);
            (Digits, Combinations, Resolution) = KeyLength(overflow_years, resolution, B);
        }

        /// <summary>
        /// Generates a short, human-readable ID that increases monotonically with time.
        /// </summary>
        /// <param name="now">timpoint at which the ID is generated</param>
        /// <returns>id</returns>
        public string Monotonic(DateTime? now = null)
        {
            DateTime n;
            if (now == null)
                n = DateTime.UtcNow;
            else
                n = ((DateTime)now).ToUniversalTime();

            double elapsed_seconds = (n - Start).TotalSeconds;
            int elapsed_intervals = (int)(elapsed_seconds / Resolution);

            return Base(elapsed_intervals, Alphabet, Digits);
        }

        /// <summary>
        /// Generates a short, human-readable ID that increases monotonically with time.
        /// </summary>
        /// <param name="resolution">maximum duration in seconds for an increment in the id</param>
        /// <param name="now">timpoint at which the ID is generated</param>
        /// <param name="alphabet">the (sorted) characters to be used in the ID generation</param>
        /// <param name="start">beginning of timeline</param>
        /// <param name="overflow_years">number of years after which the key length will increase by 1</param>
        /// <returns></returns>
        public static string Monotonic(double resolution = Hagelkorn.Resolution.Seconds, DateTime? now=null, string alphabet = DEFAULT_ALPHABET, DateTime? start = null, double overflow_years = 10)
        {
            // clean up input arguments
            DateTime n;
            if (now == null)
                n = DateTime.UtcNow;
            else
                n = ((DateTime)now).ToUniversalTime();

            DateTime s;
            if (start == null)
                s = new DateTime(2018, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            else
                s = ((DateTime)start).ToUniversalTime();

            // find parameters
            int B = alphabet.Length;
            (int digits, long combinations, double res) = KeyLength(overflow_years, resolution, B);

            // find the interval number
            double elapsed_seconds = (n - s).TotalSeconds;
            int elapsed_intervals = (int)(elapsed_seconds / res);

            // encode
            return Base(elapsed_intervals, alphabet, digits);
        }

        /// <summary>
        /// Determines some key parameters for ID generation.
        /// </summary>
        /// <param name="overflow_years">number of years after which the key length will be exceeded</param>
        /// <param name="resolution">maximum length of an interval (in seconds)</param>
        /// <param name="B">base of the positional notation (length of alphabet)</param>
        /// <returns>
        /// D (int): number of digits of the ID
        /// K (int): total number of unique IDs(intervals)
        /// T (double): duration of one interval in seconds
        /// </returns>
        public static (int, long, double) KeyLength(double overflow_years, double resolution, int B)
        {
            double total_seconds = overflow_years * 31536000;
            double K_min = total_seconds / resolution;
            int D = 1;
            long K = B;
            while (K < K_min)
            {
                D += 1;
                K *= B;
            }
            double T = total_seconds / K;
            return (D, K, T);
        }

        /// <summary>
        /// Converts a real-valued number into its baseN-notation.
        /// </summary>
        /// <param name="n">number to be converted (decimal precision will be droped)</param>
        /// <param name="alphabet">alphabet of the positional notation system</param>
        /// <param name="digits">number of digits in the ID</param>
        /// <returns>id (length may exceed the specified number of digits if n results in an overflow)</returns>
        public static string Base(double n, string alphabet, int digits)
        {
            int B = alphabet.Length;
            List<char> output = new List<char>();
            while (n > 0)
            {
                output.Add(alphabet[(int)(n % B)]);
                n = (int)(n / B);
            }
            output.Reverse();
            return new string(output.ToArray()).PadLeft(digits, alphabet[0]);
        }

        /// <summary>
        /// Generates a random alphanumberic ID.
        /// </summary>
        /// <param name="digits">length of the generated ID</param>
        /// <param name="alphabet">available characters for the ID</param>
        /// <returns></returns>
        public static string Random(int digits=5, string alphabet = DEFAULT_ALPHABET)
        {
            char[] output = new char[digits];
            for (int i = 0; i < digits; i++)
            {
                output[i] = alphabet[rnd.Next(0, alphabet.Length)];
            }
            return new string(output);
        }
    }

    /// <summary>
    /// Helper type for specifying minimum time resolutions.
    /// </summary>
    public static class Resolution
    {
        public const double Seconds = 1;
        public const double Microseconds = 1e-6;
        public const double Milliseconds = 1e-3;
        public const double Minutes = 60;
        public const double Hours = 3600;
        public const double Days = 86400;
    }
}
