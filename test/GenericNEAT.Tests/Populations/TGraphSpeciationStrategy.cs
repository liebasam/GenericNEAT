using System;
using GenericNEAT.Chromosomes;
using GeneticSharp.Domain.Chromosomes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericNEAT.Populations.Tests
{
    [TestClass]
    public class TGraphSpeciationStrategy
    {
        static readonly Func<IChromosome, IChromosome, double> Dist = (a, b) => 1;
        static GraphChromosome Graph1, Graph2;
        static int nVertMismatch, nEdgeMismatch;
       
        [ClassInitialize]
        public static void ClassInit(TestContext tc)
        {
            var chrom = new IntegerChromosome(0, 1);
            Graph1 = new GraphChromosome(chrom, chrom, 2);
            Graph1.AddVertex(0u);
            Graph1.AddVertex(2u);
            Graph1.AddEdge(0u, 2u);
            Graph1.AddEdge(2u, 0u);
            Graph2 = new GraphChromosome(chrom, chrom, 4);
            Graph2.AddVertex(0u);
            Graph2.AddVertex(1u);
            Graph2.AddVertex(2u);
            Graph2.AddVertex(3u);
            Graph2.AddEdge(0u, 2u);
            Graph2.AddEdge(1u, 2u);
            Graph2.AddEdge(2u, 3u);

            nVertMismatch = 2;
            nEdgeMismatch = 3;
        }

        [TestMethod]
        public void Constructors()
        {
            Assert.ThrowsException<ArgumentNullException>(
                () => new GraphSpeciationStrategy(0, 0, 0, null, Dist));
            Assert.ThrowsException<ArgumentNullException>(
                () => new GraphSpeciationStrategy(0, 0, 0, Dist, null));
            var strat = new GraphSpeciationStrategy(1, 2, 3, Dist, Dist);
            Assert.AreEqual(1, strat.DistanceThreshold);
            Assert.AreEqual(2, strat.VertexMatchWeight);
            Assert.AreEqual(2, strat.EdgeMatchWeight);
            Assert.AreEqual(3, strat.VertexMismatchWeight);
            Assert.AreEqual(3, strat.EdgeMismatchWeight);
            Assert.AreSame(Dist, strat.VertexDistanceFunc);
            Assert.AreSame(Dist, strat.EdgeDistanceFunc);
        }

        [TestMethod]
        public void DistanceBetween()
        {
            var strat = new GraphSpeciationStrategy(1, 10, 100, 1000, 10000, Dist, Dist);
            double expVertDist = (1 * 10)  + (nVertMismatch * 1000 / 4);
            double expEdgeDist = (1 * 100) + (nEdgeMismatch * 10000 / 3);
            double expected = expVertDist + expEdgeDist;
            double actualDist = strat.DistanceBetween(Graph1, Graph2);
            Assert.AreEqual(expected, actualDist, float.Epsilon);
        }
    }
}
