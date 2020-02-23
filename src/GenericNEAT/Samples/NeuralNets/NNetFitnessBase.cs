using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Fitnesses;
using System;

namespace GenericNEAT.Samples.NeuralNets
{
    public abstract class NNetFitnessBase : IFitness
    {
        public double Evaluate(IChromosome chromosome)
        {
            if (chromosome is null)
                throw new ArgumentNullException(nameof(chromosome));
            if (!(chromosome is NNetChromosome graph))
                throw new InvalidOperationException();
            return Evaluate(new NNet(graph));
        }

        /// <summary>
        /// Returns the fitness of the given neural network.
        /// </summary>
        public abstract double Evaluate(IBlackBox net);
    }
}
