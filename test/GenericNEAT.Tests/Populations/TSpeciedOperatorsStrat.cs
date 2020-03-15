using System;
using GeneticSharp.Domain.Chromosomes;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeneticSharp.Domain.Crossovers;
using System.Collections.Generic;

namespace GenericNEAT.Populations.Tests
{
    [TestClass]
    public class TSpeciedOperatorsStrat : SpeciedOperatorsStrategy
    {
        SpeciedPopulation Population;
        Specie Specie0, Specie1, OldSpecie;
        ICrossover Crossover;

        [TestInitialize]
        public void TestInit()
        {
            int minSpecieSize = 2;
            int maxSize = 12;

            Population = new SpeciedPopulation(
                minSize: maxSize,
                maxSize: maxSize,
                adamChromosome: new FloatingPointChromosome(0, 1, 32, 2),
                new SpeciationStrategy((a, b) => Math.Abs(TestHelpers.GetValue(a) - TestHelpers.GetValue(b)), 0.5),
                minSpecieSize: minSpecieSize);
            Population.CreateInitialGeneration();
            double[] minValue = new double[] { 0 };
            double[] maxValue = new double[] { 1 };
            int[] totalBits = new int[] { 32 };
            int[] fractionDigits = new int[] { 1 };
            Population.Species[0].Centroid = new FloatingPointChromosome(
                minValue, maxValue,
                totalBits, fractionDigits,
                new double[] { 0.0 });
            Population.Species.Add(new Specie(minSpecieSize, maxSize, 1u, new FloatingPointChromosome(
                minValue, maxValue,
                totalBits, fractionDigits,
                new double[] { 1.0 })));
            Specie0 = Population.Species[0];
            Specie1 = Population.Species[1];

            // Divide up the species
            Population.CreateNewGeneration(Population.CurrentGeneration.Chromosomes);
            Assert.AreEqual(2, Population.Species.Count);
            
            // Set Specie fitnesses
            TestHelpers.SetFitnessesAndOrder(Specie0, 0);
            TestHelpers.SetFitnessesAndOrder(Specie1, 1);

            // Create an older specie for testing
            OldSpecie = new Specie(minSpecieSize, maxSize, 2u, new FloatingPointChromosome(minValue, maxValue, totalBits, fractionDigits));
            OldSpecie.CreateInitialGeneration();
            TestHelpers.SetFitnessesAndOrder(OldSpecie, 0);
            for (int i = 1; i < 15; i++)
            {
                OldSpecie.CreateNewGeneration(OldSpecie.CurrentGeneration.Chromosomes);
                // Set the best chromosome to a different member
                OldSpecie.CurrentGeneration.Chromosomes[i % OldSpecie.CurrentGeneration.Chromosomes.Count].Fitness = i;
                OldSpecie.EndCurrentGeneration();
            }

            Crossover = new UniformCrossover(0);
        }

        #region SpecieShouldReproduce
        [TestMethod]
        public void ShouldReproduce_YoungSpecie()
        {
            Assert.IsTrue(base.SpecieShouldReproduce(Specie0));
            Assert.IsTrue(base.SpecieShouldReproduce(Specie1));
        }

        [TestMethod]
        public void ShouldReproduce_OldSpecieNoImprovement()
        {
            OldSpecie.CurrentGeneration.BestChromosome.Fitness = 0;
            Assert.IsFalse(base.SpecieShouldReproduce(OldSpecie));
        }

        [TestMethod]
        public void ShouldReproduce_OldSpecieImproved()
        {
            OldSpecie.CurrentGeneration.BestChromosome.Fitness = 100;
            Assert.IsTrue(base.SpecieShouldReproduce(OldSpecie));
        }
        #endregion

        #region CalcNewSpecieSizes
        [TestMethod]
        public void CalcNewSizes_Case50_50()
        {
            TestHelpers.SetFitnessesAndOrder(Specie0, 1);
            TestHelpers.SetFitnessesAndOrder(Specie1, 1);
            var sizes = CalcNewSpecieSizes(Population);
            Assert.AreEqual(2, sizes.Count);
            Assert.AreEqual(6, sizes[Specie0]);
            Assert.AreEqual(6, sizes[Specie1]);
        }

        [TestMethod]
        public void CalcNewSizes_Case0_100()
        {
            TestHelpers.SetFitnessesAndOrder(Specie0, 0);
            TestHelpers.SetFitnessesAndOrder(Specie1, 1);
            var sizes = CalcNewSpecieSizes(Population);
            Assert.AreEqual(2, sizes.Count);
            Assert.AreEqual(0, sizes[Specie0]);
            Assert.AreEqual(12, sizes[Specie1]);
        }

        [TestMethod]
        public void CalcNewSizes_Case40_50_trim()
        {
            TestHelpers.SetFitnessesAndOrder(Specie0, 0.4);
            TestHelpers.SetFitnessesAndOrder(Specie1, 0.5);
            var sizes = CalcNewSpecieSizes(Population);
            Assert.AreEqual(2, sizes.Count);
            Assert.AreEqual(6, sizes[Specie0]);
            Assert.AreEqual(6, sizes[Specie1]);
        }

        [TestMethod]
        public void CalcNewSizes_Case10_90_expand()
        {
            TestHelpers.SetFitnessesAndOrder(Specie0, 0.2);
            TestHelpers.SetFitnessesAndOrder(Specie1, 0.8);
            int tmp = Population.MinSpecieSize;
            Population.MinSpecieSize = 5;
            var sizes = CalcNewSpecieSizes(Population);
            Population.MinSpecieSize = tmp;
            Assert.AreEqual(2, sizes.Count);
            Assert.AreEqual(0, sizes[Specie0]);
        }

        [TestMethod]
        public void CalcNewSizes_Case0_0()
        {
            TestHelpers.SetFitnessesAndOrder(Specie0, 0);
            TestHelpers.SetFitnessesAndOrder(Specie1, 0);
            var sizes = CalcNewSpecieSizes(Population);
            Assert.AreEqual(0, sizes.Count);
        }
        #endregion

        #region PerformCrossSpecie
        [TestMethod]
        public void PerformCrossSpecie_Specie0()
        {
            var s0Parents = Specie0.CurrentGeneration.Chromosomes.Take(3).ToList();
            var s1Parents = Specie1.CurrentGeneration.Chromosomes.Take(3);
            var allParents = s0Parents.Concat(s1Parents).ToList();
            var parentsLookup = allParents.ToLookup(parent => s0Parents.Contains(parent) ? Specie0 : Specie1);
            List<IChromosome> children;
            children = base.PerformCrossSpecie(Population, Specie0, Crossover, 2, s0Parents, parentsLookup).ToList();
            Assert.AreEqual(2, children.Count);
            double child0 = TestHelpers.GetValue(children[0]);
            double child1 = TestHelpers.GetValue(children[1]);
            Assert.IsTrue(s0Parents.Any(p => TestHelpers.GetValue(p) == child0));
            Assert.IsTrue(s0Parents.Any(p => TestHelpers.GetValue(p) == child1));
        }

        [TestMethod]
        public void PerformCrossSpecie_Specie1()
        {
            var s0Parents = Specie0.CurrentGeneration.Chromosomes.Take(3);
            var s1Parents = Specie1.CurrentGeneration.Chromosomes.Take(3).ToList();
            var allParents = s0Parents.Concat(s1Parents).ToList();
            var parentsLookup = allParents.ToLookup(parent => s0Parents.Contains(parent) ? Specie0 : Specie1);
            List<IChromosome> children;
            children = base.PerformCrossSpecie(Population, Specie1, Crossover, 2, s1Parents, parentsLookup).ToList();
            Assert.AreEqual(2, children.Count);
            double child0 = TestHelpers.GetValue(children[0]);
            double child1 = TestHelpers.GetValue(children[1]);
            Assert.IsTrue(s1Parents.Any(p => TestHelpers.GetValue(p) == child0));
            Assert.IsTrue(s1Parents.Any(p => TestHelpers.GetValue(p) == child1));
        }

        [TestMethod]
        public void PerformInterspecieCross()
        {
            var s0Parents = Specie0.CurrentGeneration.Chromosomes.Take(3);
            var s1Parents = Specie1.CurrentGeneration.Chromosomes.Take(3);
            var allParents = s0Parents.Concat(s1Parents).ToList();
            List<IChromosome> children;
            children = PerformInterspecieCross(Crossover, allParents).ToList();
            Assert.AreEqual(2, children.Count);
            double c0 = TestHelpers.GetValue(children[0]);
            double c1 = TestHelpers.GetValue(children[1]);
            if (s0Parents.Any(p => TestHelpers.GetValue(p) == c0)) {
                if (!s1Parents.Any(p => TestHelpers.GetValue(p) == c1)) {
                    Assert.Fail(string.Format("Child1 {0} is not from Specie1 as expected.", c1));
                }
            }
            else if (s1Parents.Any(p => TestHelpers.GetValue(p) == c0)) {
                if (!s0Parents.Any(p => TestHelpers.GetValue(p) == c1)) {
                    Assert.Fail(string.Format("Child1 {0} is not from Specie0 as expected.", c1));
                }
            }
            else {
                Assert.Fail("Child0 is in neither Specie0 or Specie1.");
            }
        }
        #endregion

        #region PerformCrossPopulation
        [TestMethod]
        public void PerformCrossPopulation_Case50_50()
        {
            TestHelpers.SetFitnessesAndOrder(Population, 1);
            int expectedSize = Population.CurrentGeneration.Chromosomes.Count;
            var parents = Specie0.CurrentGeneration.Chromosomes.Concat(
                Specie1.CurrentGeneration.Chromosomes).ToList();
            IList<IChromosome> children = PerformCrossPopulation(Population, Crossover, 0.5f, parents);
            Assert.AreEqual(Population.CurrentGeneration.Chromosomes.Count, children.Count);
            int specie0Count = children.Count(child => Specie0.CurrentGeneration.Chromosomes.Any(
                c => TestHelpers.GetValue(c) == TestHelpers.GetValue(child)));
            Assert.AreEqual(expectedSize / 2, specie0Count);
            int specie1Count = children.Count(child => Specie1.CurrentGeneration.Chromosomes.Any(
                c => TestHelpers.GetValue(c) == TestHelpers.GetValue(child)));
            Assert.AreEqual(expectedSize / 2, specie1Count);
            Assert.AreEqual(expectedSize, specie0Count + specie1Count);
        }

        [TestMethod]
        public void PerformCrossPopulation_Case40_50()
        {
            TestHelpers.SetFitnessesAndOrder(Specie0, 0.4);
            TestHelpers.SetFitnessesAndOrder(Specie1, 0.5);
            int expectedSize = Population.CurrentGeneration.Chromosomes.Count;
            var parents = Specie0.CurrentGeneration.Chromosomes.Concat(
                Specie1.CurrentGeneration.Chromosomes).ToList();
            IList<IChromosome> children = PerformCrossPopulation(Population, Crossover, 0.5f, parents);
            Assert.AreEqual(Population.CurrentGeneration.Chromosomes.Count, children.Count);
            // (PUZZLE) How to check number in each species?
            int specie0Count = children.Count(child => Specie0.CurrentGeneration.Chromosomes.Any(
                c => TestHelpers.GetValue(c) == TestHelpers.GetValue(child)));
            Assert.AreEqual(expectedSize / 2, specie0Count);
            int specie1Count = children.Count(child => Specie1.CurrentGeneration.Chromosomes.Any(
                c => TestHelpers.GetValue(c) == TestHelpers.GetValue(child)));
            Assert.AreEqual(expectedSize / 2, specie1Count);
            Assert.AreEqual(expectedSize, specie0Count + specie1Count);
        }

        [TestMethod]
        public void PerformCrossPopulation_Case0_100()
        {
            TestHelpers.SetFitnessesAndOrder(Specie0, 1);
            TestHelpers.SetFitnessesAndOrder(Specie1, 0);
            int expectedSize = Population.CurrentGeneration.Chromosomes.Count;
            var parents = Specie0.CurrentGeneration.Chromosomes.Concat(
                Specie1.CurrentGeneration.Chromosomes).ToList();
            IList<IChromosome> children = PerformCrossPopulation(Population, Crossover, 0.5f, parents);
            Assert.AreEqual(expectedSize, children.Count);
            int specie0Count = children.Count(child => Population.SpeciationStrategy.AreSameSpecies(child, Specie0.Centroid));
            Assert.AreEqual(expectedSize, specie0Count);
            int specie1Count = children.Count(child => Population.SpeciationStrategy.AreSameSpecies(child, Specie1.Centroid));
            Assert.AreEqual(0, specie1Count);
            Assert.AreEqual(expectedSize, specie0Count + specie1Count);
        }
        #endregion
    }
}
