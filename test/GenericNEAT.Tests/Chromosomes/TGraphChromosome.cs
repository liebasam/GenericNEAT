using System;
using GeneticSharp.Domain.Chromosomes;
using LiebasamUtils.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericNEAT.Chromosomes.Tests
{
    [TestClass]
    public class TGraphChromosome
    {
        static IChromosome VertTemplate = new FloatingPointChromosome(0, 1, 4);
        static IChromosome EdgeTemplate = new FloatingPointChromosome(10, 11, 4);
        static Vertex<IChromosome>[] Verts = new Vertex<IChromosome>[]
        {
            new Vertex<IChromosome>(0, VertTemplate)
        };
        static Edge<IChromosome>[] Edges = new Edge<IChromosome>[]
        {
            new Edge<IChromosome>(0, 0, EdgeTemplate)
        };

        [TestClass]
        public class Constructors
        {
            [TestMethod]
            public void Failure()
            {
                Assert.ThrowsException<ArgumentNullException>(
                    () => new GraphChromosome(null, EdgeTemplate));
                Assert.ThrowsException<ArgumentNullException>(
                    () => new GraphChromosome(VertTemplate, null));
                Assert.ThrowsException<ArgumentOutOfRangeException>(
                    () => new GraphChromosome(VertTemplate, EdgeTemplate, -1));
                Assert.ThrowsException<ArgumentNullException>(
                    () => new GraphChromosome(VertTemplate, EdgeTemplate, null, Edges));
                Assert.ThrowsException<ArgumentNullException>(
                    () => new GraphChromosome(VertTemplate, EdgeTemplate, Verts, null));
            }

            [TestMethod]
            public void TwoArgs()
            {
                var g = new GraphChromosome(VertTemplate, EdgeTemplate);
                Assert.AreEqual(VertTemplate, g.VertexTemplate);
                Assert.AreEqual(EdgeTemplate, g.EdgeTemplate);
            }

            [TestMethod]
            public void ThreeArgs()
            {
                var g = new GraphChromosome(VertTemplate, EdgeTemplate, 1);
                Assert.AreEqual(VertTemplate, g.VertexTemplate);
                Assert.AreEqual(EdgeTemplate, g.EdgeTemplate);
            }

            [TestMethod]
            public void FourArgs()
            {
                var g = new GraphChromosome(VertTemplate, EdgeTemplate, Verts, Edges);
                Assert.AreEqual(VertTemplate, g.VertexTemplate);
                Assert.AreEqual(EdgeTemplate, g.EdgeTemplate);
                Assert.AreEqual(1, g.VertexCount);
                Assert.AreEqual(1, g.EdgeCount);
                Assert.IsTrue(g.ContainsVertex(0));
                Assert.IsTrue(g.ContainsEdge(0, 0));
            }
        }

        [TestClass]
        public class Methods
        {
            static GraphChromosome g;
            [ClassInitialize]
            public static void ClassInit(TestContext tc)
            {
                g = new GraphChromosome(VertTemplate, EdgeTemplate, Verts, Edges);
            }

            [TestMethod]
            public void Clone()
            {
                var g2 = g.Clone() as GraphChromosome;
                Assert.IsNotNull(g2);
                Assert.AreNotSame(g, g2);
                Assert.AreEqual(1, g2.VertexCount);
                Assert.AreEqual(1, g2.EdgeCount);
                Assert.AreNotSame(g[0], g2[0]);
                Assert.AreNotSame(g[0, 0], g2[0, 0]);
            }

            [TestMethod]
            public void CreateNew()
            {
                var g2 = g.CreateNew() as GraphChromosome;
                Assert.IsNotNull(g2);
                Assert.AreNotSame(g, g2);
                Assert.AreEqual(1, g2.VertexCount);
                Assert.AreEqual(1, g2.EdgeCount);
                Assert.AreNotEqual(g[0].ToString(), g2[0].ToString());
                Assert.AreNotEqual(g[0, 0].ToString(), g2[0, 0].ToString());
            }

            [TestMethod]
            public void AddVertex()
            {
                var v1 = g[0];
                g.AddVertex(0);
                var v2 = g[0];
                Assert.AreNotEqual(v1.ToString(), v2.ToString());
            }

            [TestMethod]
            public void AddEdge()
            {
                var e1 = g[0, 0];
                g.AddEdge(0, 0);
                var e2 = g[0, 0];
                Assert.AreNotEqual(e1.ToString(), e2.ToString());
            }
        }
    }
}
