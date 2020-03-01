using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Randomizations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GenericNEAT.Populations
{
    public class SpeciedPopulation : Population
    {
        readonly List<Specie> m_species;

        /// <summary>
        /// Enumerates the species in the population.
        /// </summary>
        public ICollection<Specie> Species => m_species;

        /// <summary>
        /// Number of species in the population.
        /// </summary>
        public int SpecieCount => m_species.Count;

        /// <summary>
        /// Minimum size of any specie.
        /// </summary>
        public int MinSpecieSize { get; set; }

        public ISpeciationStrategy SpeciationStrategy { get; set; }

        /// <summary>
        /// ID that will be used when creating a new specie.
        /// </summary>
        protected uint NextSpecieID { get; set; }

        /// <summary>
        /// Creates a new specied population.
        /// </summary>
        public SpeciedPopulation(int minSize, int maxSize, IChromosome adamChromosome,
            ISpeciationStrategy speciationStrategy, int minSpecieSize) : base(minSize, maxSize, adamChromosome)
        {
            if (speciationStrategy is null)
                throw new ArgumentNullException(nameof(speciationStrategy));
            if (minSpecieSize < 0)
                throw new ArgumentOutOfRangeException(nameof(minSpecieSize));
            SpeciationStrategy = speciationStrategy;
            MinSpecieSize = minSpecieSize;
        }

        #region Methods
        public override void CreateInitialGeneration()
        {
            // Reset the species list
            m_species.Clear();
            var specie = new Specie(MinSpecieSize, MinSize, 0u, AdamChromosome);
            specie.CreateInitialGeneration();
            m_species.Add(specie);
            NextSpecieID = 1u;

            // Reset the base generations list
            Generations.Clear();
            base.CreateNewGeneration(specie.CurrentGeneration.Chromosomes);
        }

        public override void CreateNewGeneration(IList<IChromosome> chromosomes)
        {
            base.CreateNewGeneration(chromosomes);

            // Organize chromosomes by species
            var lookup = chromosomes.ToLookup(c => GetSpecie(c));
            foreach (var group in lookup)
            {
                // Add em to the correct specie
                group.Key.CreateNewGeneration(group.ToList());
            }

            // Remove empty species
            for (int i = 0; i < SpecieCount; i++)
            {
                if (m_species[i].CurrentGeneration.Chromosomes.Count == 0)
                {
                    m_species.RemoveAt(i);
                    i--;
                }
            }
        }

        public override void EndCurrentGeneration()
        {
            base.EndCurrentGeneration();
            for (int i = 0; i < SpecieCount; i++)
                m_species[i].EndCurrentGeneration();
        }

        /// <summary>
        /// Returns the first specie to which <paramref name="chromosome"/>
        /// fits, according too <see cref="SpeciationStrategy"/>.
        /// </summary>
        protected Specie GetSpecie(IChromosome chromosome)
        {
            for (int i = 0; i < SpecieCount; i++)
            {
                var curSpecie = m_species[i];
                if (SpeciationStrategy.AreSameSpecies(chromosome, curSpecie.Centroid))
                    return curSpecie;
            }

            // Create a new specie
            var specie = new Specie(MinSpecieSize, MaxSize, ++NextSpecieID, chromosome);
            m_species.Add(specie);
            return specie;
        }
        #endregion
    }
}
