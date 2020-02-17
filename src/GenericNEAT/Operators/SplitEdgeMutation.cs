using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Randomizations;
using LiebasamUtils.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GenericNEAT.Operators
{
    public sealed class SplitEdgeMutation : IMutation
    {
        IDFactory Factory { get; set; }

        public SplitEdgeMutation(IDFactory factory)
        {
            if (factory is null)
                throw new ArgumentNullException(nameof(factory));
            Factory = factory;
        }

        public bool IsOrdered => true;

        public void Mutate(IChromosome chromosome, float probability)
        {
            if (!(chromosome is GraphChromosomeBase graph))
                throw new InvalidOperationException();
            if (RandomizationProvider.Current.GetDouble() < probability)
            {
                var edge = graph.Edges.Shuffle(RandomizationProvider.Current).First();
                SplitEdge(graph, edge);
            }
        }

        void SplitEdge(GraphChromosomeBase graph, Edge<IChromosome> edge)
        {
            uint newID = Factory.GetID(edge.IDFrom, edge.IDTo);
            graph.RemoveEdge(edge.IDFrom, edge.IDTo);
            graph.AddVertex(newID, graph.GenerateVertex());
            graph.AddEdge(edge.IDFrom, newID, edge.Value);
            graph.AddEdge(newID, edge.IDTo, graph.GenerateEdge());
        }
    }
}
