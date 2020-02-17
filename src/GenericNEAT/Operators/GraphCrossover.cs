using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Randomizations;
using System;
using System.Collections.Generic;

namespace GenericNEAT.Operators
{
    public sealed class GraphCrossover : ICrossover
    {
        public ICrossover VertexCrossover { get; set; }
        public ICrossover EdgeCrossover { get; set; }

        public int ParentsNumber => 2;
        public int ChildrenNumber => 1;
        public int MinChromosomeLength => 1;
        public bool IsOrdered => true;

        public GraphCrossover(ICrossover vertexCrossover, ICrossover edgeCrossover)
        {
            if (vertexCrossover is null)
                throw new ArgumentNullException(nameof(vertexCrossover));
            if (edgeCrossover is null)
                throw new ArgumentNullException(nameof(edgeCrossover));
            VertexCrossover = vertexCrossover;
            EdgeCrossover = edgeCrossover;
        }

        /// <summary>
        /// Cross two graphs, assuming the first has a greater fitness.
        /// </summary>
        /// <param name="parents"></param>
        /// <returns></returns>
        public IList<IChromosome> Cross(IList<IChromosome> parents)
        {
            var p1 = parents[0] as GraphChromosome;
            var p2 = parents[1] as GraphChromosome;
            if (p1 is null || p2 is null)
                throw new InvalidOperationException();

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

            var ret = new List<IChromosome>(1);
            ret.Add(child);
            return ret;
        }
    }
}
