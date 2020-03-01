using GenericNEAT.Chromosomes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericNEAT.Operators.Tests
{
    [TestClass]
    public class TSplitEdgeMutation
    {
        GraphChromosome Graph;

        [TestInitialize]
        public void TestInit()
        {
            Graph = new GraphChromosome(TestHelpers.IntegerChromosome(), TestHelpers.IntegerChromosome());
            Graph.AddVertex(0u);
            Graph.AddVertex(1u);
            Graph.AddEdge(0u, 1u);
        }
        
        [TestMethod]
        public void Mutate()
        {
            var factory = new IDFactory(3u);
            var mut = new SplitEdgeMutation(factory);
            mut.Mutate(Graph, 0);
            Assert.AreEqual(2, Graph.VertexCount);
            Assert.AreEqual(1, Graph.EdgeCount);
            mut.Mutate(Graph, 1);
            Assert.AreEqual(3, Graph.VertexCount);
            Assert.IsTrue(Graph.ContainsVertex(3u));
            Assert.AreEqual(2, Graph.EdgeCount);
            Assert.IsTrue(Graph.ContainsEdge(0u, 3u));
            Assert.IsTrue(Graph.ContainsEdge(3u, 1u));
            Assert.IsFalse(Graph.ContainsEdge(0u, 1u));
        }
    }
}
