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
        #region Fields
        /// <summary>
        /// Enumerates the species in the population.
        /// </summary>
        public IList<Specie> Species { get; set; }

        /// <summary>
        /// Minimum size of any specie.
        /// </summary>
        public int MinSpecieSize { get; set; }

        public ISpeciationStrategy SpeciationStrategy { get; set; }

        /// <summary>
        /// ID that will be used when creating a new specie.
        /// </summary>
        protected uint NextSpecieID { get; set; }
        #endregion

        #region Constructors
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
            Species = new List<Specie>();
        }
        #endregion

        #region Methods
        public override void CreateInitialGeneration()
        {
            // Reset the species list
            Species.Clear();
            var specie = new Specie(MinSpecieSize, MinSize, 0u, AdamChromosome);
            specie.CreateInitialGeneration();
            Species.Add(specie);
            NextSpecieID = 1u;

            // Reset the base generations list
            Generations.Clear();
            GenerationsNumber = 0;
            base.CreateNewGeneration(specie.CurrentGeneration.Chromosomes);
        }

        public override void CreateNewGeneration(IList<IChromosome> chromosomes)
        {
            base.CreateNewGeneration(chromosomes);

            // Organize chromosomes by species
            var lookup = chromosomes.ToLookup(c => GetClosestSpecie(c));
            foreach (var group in lookup)
            {
                // Add em to the correct specie
                group.Key.CreateNewGeneration(group.ToList());
            }

            // Remove empty species
            for (int i = 0; i < Species.Count; i++)
            {
                if (Species[i].CurrentGeneration.Chromosomes.Count == 0)
                {
                    Species.RemoveAt(i);
                    i--;
                }
            }
        }

        public override void EndCurrentGeneration()
        {
            base.EndCurrentGeneration();
            for (int i = 0; i < Species.Count; i++)
                Species[i].EndCurrentGeneration();
        }

        /// <summary>
        /// Returns the first specie to which <paramref name="chromosome"/>
        /// fits, according too <see cref="SpeciationStrategy"/>.
        /// </summary>
        protected Specie GetClosestSpecie(IChromosome chromosome)
        {
            for (int i = 0; i < Species.Count; i++)
            {
                var curSpecie = Species[i];
                if (SpeciationStrategy.AreSameSpecies(chromosome, curSpecie.Centroid))
                    return curSpecie;
            }

            // Create a new specie
            var specie = new Specie(MinSpecieSize, MaxSize, NextSpecieID++, chromosome);
            Species.Add(specie);
            NextSpecieID++;
            return specie;
        }
        #endregion
    }
}
