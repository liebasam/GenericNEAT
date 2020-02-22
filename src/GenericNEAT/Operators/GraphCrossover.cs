using GenericNEAT.Chromosomes;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Randomizations;
using System;
using System.Collections.Generic;

namespace GenericNEAT.Operators
{
    public sealed class GraphCrossover : GraphCrossoverBase
    {
        public GraphCrossover(ICrossover vertexCrossover, ICrossover edgeCrossover) : base(vertexCrossover, edgeCrossover) { }

        protected override IGraphChromosome PerformCrossover(IGraphChromosome p1, IGraphChromosome p2) 
        {
            IList<IChromosome> parents = new IChromosome[2];

            // "Disjoint genes are inherited from the more fit parent."
            var child = p1.Clone() as GraphChromosome;
            foreach (var vert in p1.Vertices)
                if (p2.ContainsVertex(vert.ID))
                {
                    parents[0] = vert.Value;
                    parents[1] = p2[vert.ID];
                    child[vert.ID] = VertexCrossover.Cross(parents)[0];
                }

            foreach (var edge in p1.Edges)
                if (p2.ContainsEdge(edge.IDFrom, edge.IDTo))
                {
                    parents[0] = edge.Value;
                    parents[1] = p2[edge.IDFrom, edge.IDTo];
                    child[edge.IDFrom, edge.IDTo] = EdgeCrossover.Cross(parents)[0];
                }

            return child;
        }
    }
}
