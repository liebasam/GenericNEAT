using GeneticSharp.Domain.Chromosomes;

namespace GenericNEAT.Populations
{
    public interface ISpeciationStrategy
    {
        /// <summary>
        /// Returns true if the chromosomes are similar enough to
        /// be in the same species, false otherwise.
        /// </summary>
        bool AreSameSpecies(IChromosome a, IChromosome b);
    }
}
