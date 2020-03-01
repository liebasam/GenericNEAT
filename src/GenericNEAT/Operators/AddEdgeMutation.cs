using GenericNEAT.Chromosomes;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Randomizations;
using System.Linq;

namespace GenericNEAT.Operators
{
    public class AddEdgeMutation : GraphMutationBase
    {
        protected override void PerformMutation(IGraphChromosome graph, float probability)
        {
            if (RandomizationProvider.Current.GetDouble() >= probability)
                return;
            // No edges to add
            if (graph.EdgeCount == graph.VertexCount * graph.VertexCount)
                return;
            // Shuffle
            var allIDs = graph.Vertices.Select(v => v.ID);
            uint[] fromVerts = MutationService.Shuffle(allIDs, RandomizationProvider.Current).ToArray();
            uint[] toVerts = MutationService.Shuffle(allIDs, RandomizationProvider.Current).ToArray();
            // Find the first that does not exist
            foreach (var v1 in fromVerts)
                foreach (var v2 in toVerts)
                    if (!graph.ContainsEdge(v1, v2))
                    {
                        graph.AddEdge(v1, v2, graph.CreateNewEdge());
                        return;
                    }
        } 
    }
}
