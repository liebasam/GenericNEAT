using GeneticSharp.Domain.Populations;
using System.Collections.Generic;

namespace GenericNEAT.Populations
{
    public interface ISpeciedPopulation : IPopulation
    {
        /// <summary>
        /// Enumerates the species in the population.
        /// </summary>
        IList<Specie> Species { get; }

        /// <summary>
        /// Minimum size of any specie.
        /// </summary>
        int MinSpecieSize { get; set; }
    }
}
