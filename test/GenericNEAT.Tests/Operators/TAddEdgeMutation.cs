using GenericNEAT.Chromosomes;
using GeneticSharp.Domain.Chromosomes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericNEAT.Operators.Tests
{
    [TestClass]
    public class TAddEdgeMutation
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
            var mut = new AddEdgeMutation();
            mut.Mutate(Graph, 0);
            Assert.AreEqual(1, Graph.EdgeCount);
            mut.Mutate(Graph, 1);
            Assert.AreEqual(2, Graph.EdgeCount);
        }
    }
}
