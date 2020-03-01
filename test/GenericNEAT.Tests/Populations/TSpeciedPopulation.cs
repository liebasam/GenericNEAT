using System;
using System.Linq;
using System.Collections.Generic;
using GeneticSharp.Domain.Chromosomes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericNEAT.Populations.Tests
{
    [TestClass]
    public class TSpeciedPopulation
    {
        static readonly IChromosome chromosome = new FloatingPointChromosome(0, 1, 4, 1);
        static readonly ISpeciationStrategy strat = new SpeciationStrategy(
            (a, b) => Math.Abs(
                (a as FloatingPointChromosome).ToFloatingPoints()[0] - 
                (b as FloatingPointChromosome).ToFloatingPoints()[0]), 0.5);

        [TestMethod]
        public void Constructor()
        {
            Assert.ThrowsException<ArgumentNullException>(
                () => new SpeciedPopulation(5, 10, chromosome, null, 2));
            var pop = new SpeciedPopulation(5, 10, chromosome, strat, 2);
            Assert.AreEqual(0, pop.Species.Count);
            Assert.AreEqual(2, pop.MinSpecieSize);
        }

        [TestMethod]
        public void CreateInitialGeneration()
        {
            var pop = new SpeciedPopulation(5, 10, chromosome, strat, 2);
            pop.CreateInitialGeneration();
            Assert.AreEqual(1, pop.Species.Count);
            Assert.AreEqual(0u, pop.Species[0].ID);
            Assert.AreSame(chromosome, pop.Species[0].Centroid);
        }

        [TestMethod]
        public void CreateNewGeneration()
        {
            int maxSize = 20;

            var pop = new SpeciedPopulation(maxSize, maxSize, chromosome, strat, 2);
            pop.CreateInitialGeneration();
            var list = new List<IChromosome>(10);
            for (int i = 0; i < maxSize; i++)
            {
                list.Add(chromosome.CreateNew());
            }
            pop.CreateNewGeneration(list);

            Assert.AreEqual(2, pop.Species.Count);
            Assert.AreEqual(0u, pop.Species[0].ID);
            Assert.AreEqual(1u, pop.Species[1].ID);
            var concatSpecies = pop.Species[0].CurrentGeneration.Chromosomes.Concat(
                pop.Species[1].CurrentGeneration.Chromosomes).ToList();
            var chroms = pop.CurrentGeneration.Chromosomes;
            Assert.AreEqual(chroms.Count, concatSpecies.Count);
            for (int i = 0; i < concatSpecies.Count; i++)
                Assert.IsTrue(chroms.Contains(concatSpecies[i]));
        }
    }
}
