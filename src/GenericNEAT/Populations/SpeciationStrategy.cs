using GeneticSharp.Domain.Chromosomes;
using System;

namespace GenericNEAT.Populations
{
    /// <summary>
    /// Generic implementation of <see cref="SpeciationStrategyBase"/>.
    /// </summary>
    public sealed class SpeciationStrategy : SpeciationStrategyBase
    {
        /// <summary>
        /// Function to measure the distance between two chromosomes.
        /// </summary>
        public Func<IChromosome, IChromosome, double> DistanceFunction { get; set; }
        
        /// <summary>
        /// Creates a speciation strategy from a function.
        /// </summary>
        public SpeciationStrategy(Func<IChromosome, IChromosome, double> distanceFunction, double distanceThreshold) : base(distanceThreshold)
        {
            if (distanceFunction is null)
                throw new ArgumentNullException(nameof(distanceFunction));
            DistanceFunction = distanceFunction;
        }

        public override double DistanceBetween(IChromosome a, IChromosome b) => DistanceFunction(a, b);
    }
}
