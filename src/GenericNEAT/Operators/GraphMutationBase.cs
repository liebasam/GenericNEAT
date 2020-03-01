using GenericNEAT.Chromosomes;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Mutations;
using System;

namespace GenericNEAT.Operators
{
    public abstract class GraphMutationBase : IMutation
    {
        public bool IsOrdered => false;

        public void Mutate(IChromosome chromosome, float probability)
        {
            if (chromosome is null)
                throw new ArgumentNullException(nameof(chromosome));
            if (probability < 0 || probability > 1)
                throw new ArgumentOutOfRangeException(nameof(probability));
            if (!(chromosome is IGraphChromosome graph))
                throw new InvalidOperationException();
            PerformMutation(graph, probability);
        }

        /// <summary>
        /// Perform the mutation assuming that <paramref name="graph"/> exists and
        /// <paramref name="probability"/> is between 0 and 1.
        /// </summary>
        protected abstract void PerformMutation(IGraphChromosome graph, float probability);
    }
}
