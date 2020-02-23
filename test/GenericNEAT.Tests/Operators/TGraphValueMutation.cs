using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using GenericNEAT.Chromosomes;

namespace GenericNEAT.Operators.Tests
{
    [TestClass]
    public class TGraphValueMutation
    {
        GraphChromosome Graph;

        [TestInitialize]
        public void TestInit()
        {
            var verts = TestHelpers.VertexCollection(0, 2).ToArray();
            var edges = TestHelpers.EdgeCollection((0, 2)).ToArray();
            Graph = new GraphChromosome(verts[0].Value, edges[0].Value, verts, edges);
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
