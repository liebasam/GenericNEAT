using GenericNEAT.Chromosomes;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Crossovers;
using System;
using System.Collections.Generic;
using System.Text;

namespace GenericNEAT.Operators
{
    public abstract class GraphCrossoverBase : ICrossover
    {
        public ICrossover VertexCrossover { get; set; }
        public ICrossover EdgeCrossover { get; set; }

        public int ParentsNumber => 2;

        public int ChildrenNumber => 1;

        public int MinChromosomeLength => 0;

        public bool IsOrdered => false;

        public GraphCrossoverBase(ICrossover vertexCrossover, ICrossover edgeCrossover)
        {
            if (vertexCrossover is null)
                throw new ArgumentNullException(nameof(vertexCrossover));
            if (edgeCrossover is null)
                throw new ArgumentNullException(nameof(edgeCrossover));
            VertexCrossover = vertexCrossover;
            EdgeCrossover = edgeCrossover;
        }

        /// <summary>
        /// Cross two graphs, assuming that the first has a greater fitness.
        /// </summary>
        public IList<IChromosome> Cross(IList<IChromosome> parents)
        {
            var p1 = parents[0] as IGraphChromosome;
            var p2 = parents[1] as IGraphChromosome;
            if (p1 is null || p2 is null)
                throw new InvalidOperationException();
            return new IGraphChromosome[] { PerformCrossover(p1, p2) };
        }

        protected abstract IGraphChromosome PerformCrossover(IGraphChromosome p1, IGraphChromosome p2);
    }
}
