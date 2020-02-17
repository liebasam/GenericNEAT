using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Randomizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericNEAT.Operators
{
    public class AddEdgeMutation : IMutation
    {
        public bool IsOrdered => true;

        public void Mutate(IChromosome chromosome, float probability)
        {
            if (!(chromosome is GraphChromosomeBase graph))
                throw new InvalidOperationException();
            if (RandomizationProvider.Current.GetDouble() < probability)
                AddEdge(graph);
        }

        void AddEdge(GraphChromosomeBase graph)
        {
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
                        graph.AddEdge(v1, v2, graph.GenerateEdge());
                        return;
                    }
        }
    }
}
