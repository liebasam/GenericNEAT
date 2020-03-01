using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericNEAT.Populations.Tests
{
    [TestClass]
    public class TSpeciationStrategy
    {
        [TestMethod]
        public void Constructors()
        {
            var strat = new SpeciationStrategy((a, b) => 1, 3);
            Assert.AreEqual(3, strat.DistanceThreshold);
            Assert.AreEqual(1, strat.DistanceFunction(null, null));
        }

        [TestMethod]
        public void AreSameSpecia()
        {
            var strat = new SpeciationStrategy((a, b) => 1, 3);
            Assert.IsTrue(strat.AreSameSpecies(null, null));
            strat.DistanceThreshold = 0;
            Assert.IsFalse(strat.AreSameSpecies(null, null));
        }
    }
}
