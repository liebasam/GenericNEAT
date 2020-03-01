using GeneticSharp.Domain.Chromosomes;
using System;

namespace GenericNEAT.Populations
{
    /// <summary>
    /// Base class for speciation strategies.
    /// </summary>
    public abstract class SpeciationStrategyBase : ISpeciationStrategy
    {
        #region Fields
        /// <summary>
        /// Minimum distance between any two chromosomes to be
        /// considered of the same specie.
        /// </summary>
        public double DistanceThreshold { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a speciation strategy with the provided
        /// distance threshold.
        /// </summary>
        /// <param name="distanceThreshold">Minimum distance
        /// between any two chromosomes to be considered in
        /// the same specie.</param>
        public SpeciationStrategyBase(double distanceThreshold)
        {
            DistanceThreshold = distanceThreshold;
        }
        #endregion

        #region Methods
        public virtual bool AreSameSpecies(IChromosome a, IChromosome b) => 
            DistanceBetween(a, b) <= DistanceThreshold;

        /// <summary>
        /// Gets the distance between two chromosomes.
        /// </summary>
        public abstract double DistanceBetween(IChromosome a, IChromosome b);
        #endregion
    }
}
