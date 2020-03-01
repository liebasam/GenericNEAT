using GeneticSharp.Domain.Chromosomes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GenericNEAT.Populations
{
    /// <summary>
    /// Generic implementation of <see cref="ISpeciationStrategy"/>.
    /// </summary>
    public sealed class SpeciationStrategy : ISpeciationStrategy
    {
        /// <summary>
        /// Function to measure the distance between two chromosomes.
        /// </summary>
        public Func<IChromosome, IChromosome, double> DistanceFunction { get; set; }
        
        /// <summary>
        /// Minimum distance between any two chromosomes to be
        /// considered of the same specie.
        /// </summary>
        public double DistanceThreshold { get; set; }

        /// <summary>
        /// Creates a speciation strategy from a function.
        /// </summary>
        public SpeciationStrategy(Func<IChromosome, IChromosome, double> distanceFunction, double distanceThreshold)
        {
            if (distanceFunction is null)
                throw new ArgumentNullException(nameof(distanceFunction));
            DistanceFunction = distanceFunction;
            DistanceThreshold = distanceThreshold;
        }

        public bool AreSameSpecies(IChromosome a, IChromosome b) => DistanceFunction(a, b) <= DistanceThreshold;
    }
}
