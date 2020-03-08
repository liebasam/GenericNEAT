using System;
using GeneticSharp.Domain.Chromosomes;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericNEAT.Populations.Tests
{
    [TestClass]
    public class TSpeciedOperatorsStrat : SpeciedOperatorsStrategy
    {
        static SpeciedPopulation Population;
        static SpeciedOperatorsStrategy Strategy;
        static Specie Specie0, Specie1;

        [ClassInitialize]
        public static void ClassInit(TestContext tc)
        {
            int minSpecieSize = 2;
            int maxSize = 10;

            Population = new SpeciedPopulation(
                minSize: maxSize,
                maxSize: maxSize,
                adamChromosome: new FloatingPointChromosome(0, 1, 32, 1),
                new SpeciationStrategy((a, b) => Math.Abs(TestHelpers.GetValue(a) - TestHelpers.GetValue(b)), 0.5),
                minSpecieSize: minSpecieSize);
            Population.CreateInitialGeneration();
            Population.Species[0].Centroid = new FloatingPointChromosome(
                new double[] { 0 }, new double[] { 1 },
                new int[] { 32 }, new int[] { 1 },
                new double[] { 0.0 });
            Population.Species.Add(new Specie(minSpecieSize, maxSize, 1u, new FloatingPointChromosome(
                new double[] { 0 }, new double[] { 1 },
                new int[] { 32 }, new int[] { 1 },
                new double[] { 1.0 })));
            Specie0 = Population.Species[0];
            Specie1 = Population.Species[1];
            // Divide up the species
            Population.CreateNewGeneration(Population.CurrentGeneration.Chromosomes);
            Assert.AreEqual(2, Population.Species.Count);
            Strategy = new SpeciedOperatorsStrategy();
        }

        [TestClass]
        public new class SpecieShouldReproduce
        {
            [TestMethod]
            public void Case() { Assert.Fail(); }
        }

        [TestClass]
        public new class CalcNewSpecieSizes
        {
            [TestMethod]
            public void Case() { Assert.Fail(); }
        }

        [TestClass]
        public class PerformCrossSpecie
        {
            [TestMethod]
            public void Case() { Assert.Fail(); }
        }

        [TestClass]
        public class PerformCrossPopulation
        {
            [TestMethod]
            public void Case() { Assert.Fail(); }
        }
    }
}
