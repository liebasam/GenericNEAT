using System;
using System.Collections.Generic;
using System.Linq;
using GenericNEAT.Chromosomes;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Crossovers;
using LiebasamUtils.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericNEAT.Operators.Tests
{
    [TestClass]
    public class TGraphCrossover
    {
        static GraphCrossover Crossover = new GraphCrossover(
            SelectSecondParent.Instance, SelectSecondParent.Instance);
        GraphChromosome Graph1, Graph2;

        [TestInitialize]
        public void TestInit()
        {
            var verts = TestHelpers.VertexCollection(0, 2).ToArray();
            var edges = TestHelpers.EdgeCollection((0, 0), (0, 2)).ToArray();
            Graph1 = new GraphChromosome(verts[0].Value, edges[0].Value, verts, edges);

            verts = TestHelpers.VertexCollection(0, 1, 3).ToArray();
            foreach (var vert in verts)
                vert.Cast<IntegerChromosome>().Value.FlipGene(0);
            edges = TestHelpers.EdgeCollection((0, 0), (0, 1), (0, 3)).ToArray();
            foreach (var edge in edges)
                edge.Cast<IntegerChromosome>().Value.FlipGene(0);
            Graph2 = new GraphChromosome(verts[0].Value, edges[0].Value, verts, edges);
        }

        [TestMethod]
        public void CrossG1G2()
        {
            var parents = new List<IChromosome>(2);
            parents.Add(Graph1); parents.Add(Graph2);
            var child = Crossover.Cross(parents)[0] as GraphChromosome;
            Assert.AreNotSame(Graph1, child);
            Assert.AreNotSame(Graph2, child);
            Assert.AreEqual(2, child.VertexCount);
            Assert.AreEqual(2, child.EdgeCount);
            Assert.AreEqual(true, TestHelpers.AnyIsTrue(child[0]));
            Assert.AreEqual(false, TestHelpers.AnyIsTrue(child[2]));
            Assert.AreEqual(true, TestHelpers.AnyIsTrue(child[0, 0]));
            Assert.AreEqual(false, TestHelpers.AnyIsTrue(child[0, 2]));
        }

        [TestMethod]
        public void CrossG2G1()
        {
            var parents = new List<IChromosome>(2);
            parents.Add(Graph2); parents.Add(Graph1);
            var child = Crossover.Cross(parents)[0] as GraphChromosome;
            Assert.AreNotSame(Graph1, child);
            Assert.AreNotSame(Graph2, child);
            Assert.AreEqual(3, child.VertexCount);
            Assert.AreEqual(3, child.EdgeCount);
            Assert.AreEqual(false, TestHelpers.AnyIsTrue(child[0]));
            Assert.AreEqual(true, TestHelpers.AnyIsTrue(child[1]));
            Assert.AreEqual(true, TestHelpers.AnyIsTrue(child[3]));
            Assert.AreEqual(false, TestHelpers.AnyIsTrue(child[0, 0]));
            Assert.AreEqual(true, TestHelpers.AnyIsTrue(child[0, 1]));
            Assert.AreEqual(true, TestHelpers.AnyIsTrue(child[0, 3]));
        }

        public class SelectSecondParent : ICrossover
        {
            public static SelectSecondParent Instance { get; } = new SelectSecondParent();
            private SelectSecondParent() { }
            public int ParentsNumber => 2;
            public int ChildrenNumber => 1;
            public int MinChromosomeLength => 1;
            public bool IsOrdered => true;

            public IList<IChromosome> Cross(IList<IChromosome> parents)
            {
                var list = new List<IChromosome>(1);
                list.Add(parents[1].Clone());
                return list;
            }
        }
    }
}
