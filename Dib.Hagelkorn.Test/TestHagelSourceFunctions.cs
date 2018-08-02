using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dib.Hagelkorn.Test
{
    [TestClass]
    public class TestHagelSourceFunctions
    {
        [TestMethod]
        public void TestKeyLength()
        {
            (int D1, long K1, double T1) = HagelSource.KeyLength(
                                            overflow_years: 1,
                                            resolution: Resolution.Days,
                                            B: 10);
            Assert.AreEqual(3, D1);
            Assert.AreEqual(Math.Pow(10, D1), K1);
            Assert.AreEqual(3, D1);
            Assert.IsTrue(T1 < Resolution.Days);

            (int D2, long K2, double T2) = HagelSource.KeyLength(
                                            overflow_years: 30,
                                            resolution: Resolution.Days,
                                            B: 28);
            Assert.AreEqual(Math.Pow(28, D2), K2);
            Assert.IsTrue(T2 < Resolution.Days);
        }

        [TestMethod]
        public void TestBase()
        {
            Assert.AreEqual("09", HagelSource.Base(9, "0123456789ABCDEF", 2));
            Assert.AreEqual("0D", HagelSource.Base(13, "0123456789ABCDEF", 2));
            Assert.AreEqual("10", HagelSource.Base(16, "0123456789ABCDEF", 2));
            Assert.AreEqual("100", HagelSource.Base(256, "0123456789ABCDEF", 2));
        }

        [TestMethod]
        public void TestMonotonic()
        {
            string id = HagelSource.Hagelkorn(
                resolution: Resolution.Days,
                now: new DateTime(2018, 12, 31, 23, 59, 59, DateTimeKind.Utc),
                alphabet: "0123456789",
                start: new DateTime(2018, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                overflow_years: 1
                );

            Assert.AreEqual("999", id);
        }

        [TestMethod]
        public void TestEquivalence()
        {
            var hs = new HagelSource();
            var now = DateTime.Now;
            string hs_monotonic = hs.Monotonic(now);
            string hs_hagelkorn = HagelSource.Hagelkorn(now: now);
            Assert.AreEqual(hs_monotonic, hs_hagelkorn);
        }

        [TestMethod]
        public void TestRandom()
        {
            HashSet<string> ids = new HashSet<string>();
            for (int i = 0; i < 100; i++)
            {
                ids.Add(HagelSource.Random());
            }
            Assert.AreEqual(100, ids.Count);
        }
    }
}
