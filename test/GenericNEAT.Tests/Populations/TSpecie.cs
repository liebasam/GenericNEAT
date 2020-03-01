using System;
using GeneticSharp.Domain.Chromosomes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericNEAT.Populations.Tests
{
    [TestClass]
    public class TSpecie
    {
        [TestMethod]
        public void Constructor()
        {
            Assert.ThrowsException<ArgumentNullException>(
                () => new Specie(2, 3, 0u, null));
            var adam = new IntegerChromosome(0, 1);
            var s = new Specie(2, 10, 5u, adam);
            Assert.AreEqual(5u, s.ID);
            Assert.AreSame(adam, s.Centroid);
        }

        [TestMethod]
        public void CreateInitialGeneration()
        {
            var s = new Specie(2, 10, 0u, new IntegerChromosome(0, 1));
            s.CreateInitialGeneration();
            Assert.AreEqual(10, s.CurrentGeneration.Chromosomes.Count);
        }
    }
}
