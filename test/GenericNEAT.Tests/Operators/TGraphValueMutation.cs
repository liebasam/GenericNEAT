using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Chromosomes;
using LiebasamUtils.Collections;
using System.Linq;

namespace GenericNEAT.Operators.Tests
{
    [TestClass]
    public class TGraphValueMutation
    {
        GraphChromosome Graph;

        [TestInitialize]
        public void TestInit()
        {
            var verts = TestHelpers.VertexCollection(0, 2);
            var edges = TestHelpers.EdgeCollection((0, 2));
            Graph = new GraphChromosome(verts, edges);
        }

        

        [TestMethod]
        public void MutateVertices()
        {
            var mut = new GraphValueMutation(
                vertexMutation: TestHelpers.UniformMutation, 1,
                edgeMutation: TestHelpers.UniformMutation, 0);
            mut.Mutate(Graph, 1);
            Assert.AreEqual(true, TestHelpers.AnyIsTrue(Graph[0]));
            Assert.AreEqual(true, TestHelpers.AnyIsTrue(Graph[2]));
            Assert.AreEqual(false, TestHelpers.AnyIsTrue(Graph[0, 2]));
        }

        [TestMethod]
        public void MutateEdges()
        {
            var mut = new GraphValueMutation(
                vertexMutation: TestHelpers.UniformMutation, 0,
                edgeMutation: TestHelpers.UniformMutation, 1);
            mut.Mutate(Graph, 1);
            Assert.AreEqual(false, TestHelpers.AnyIsTrue(Graph[0]));
            Assert.AreEqual(false, TestHelpers.AnyIsTrue(Graph[2]));
            Assert.AreEqual(true, TestHelpers.AnyIsTrue(Graph[0, 2]));
        }
    }
}
