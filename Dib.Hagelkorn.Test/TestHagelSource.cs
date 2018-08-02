using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dib.Hagelkorn.Test
{
    [TestClass]
    public class TestHagelSource
    {
        [TestMethod]
        public void TestConstructor()
        {
            var hs = new HagelSource(overflow_years: 42);
            Assert.AreEqual((hs.End - hs.Start).TotalSeconds, hs.TotalSeconds);
            Assert.AreEqual(42 * 31536000, hs.TotalSeconds);
            Assert.AreEqual(hs.Alphabet.Length, hs.B);
            Assert.AreEqual(hs.Combinations, Math.Pow(hs.B, hs.Digits));
            Assert.AreEqual(hs.Resolution, hs.TotalSeconds / hs.Combinations);
        }

        [TestMethod]
        public void TestMonotonic()
        {
            var hs = new HagelSource(
                resolution: Resolution.Days,
                alphabet: "0123456789",
                start: new DateTime(2018, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                overflow_years: 1);
            var id = hs.Monotonic(now: new DateTime(2018, 12, 31, 23, 59, 59, DateTimeKind.Utc));

            Assert.AreEqual(hs.Digits, id.Length);
            Assert.AreEqual("999", id);
        }

        [TestMethod]
        public void TestT0()
        {
            var hs = new HagelSource();
            var first = hs.Monotonic(now: hs.Start);
            Assert.IsTrue(first.All(c => c == hs.Alphabet[0]));
        }

        [TestMethod]
        public void TestTOverflow()
        {
            var hs = new HagelSource();
            var overflow = hs.Monotonic(now: hs.End);
            Assert.AreEqual(hs.Digits + 1, overflow.Length);
            string expected = hs.Alphabet[1].ToString() + new string(hs.Alphabet[0], hs.Digits);
            Assert.AreEqual(expected, overflow);
        }
    }
}
