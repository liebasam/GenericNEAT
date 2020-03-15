using GeneticSharp.Domain;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Randomizations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GenericNEAT.Populations
{
    public class SpeciedOperatorsStrategy : IOperatorsStrategy
    {
        /// <summary>
        /// Odds that any crossover is done across species.
        /// </summary>
        public double InterspecieMatingRate { get; set; }

        public SpeciedOperatorsStrategy(double interspecieMatingRate = 0)
        {
            if (interspecieMatingRate < 0 || interspecieMatingRate > 1)
                throw new ArgumentOutOfRangeException(nameof(interspecieMatingRate));
            InterspecieMatingRate = interspecieMatingRate;
        }

        #region Methods
        public IList<IChromosome> Cross(IPopulation population, ICrossover crossover, float crossoverProbability, IList<IChromosome> parents)
        {
            var pop = population as ISpeciedPopulation;
            if (pop is null)
                throw new InvalidOperationException();
            return PerformCrossPopulation(pop, crossover, crossoverProbability, parents);
        }

        public void Mutate(IMutation mutation, float mutationProbability, IList<IChromosome> chromosomes)
        {
            for (int i = 0; i < chromosomes.Count; i++)
                mutation.Mutate(chromosomes[i], mutationProbability);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="population"></param>
        /// <param name="crossover"></param>
        /// <param name="crossoverProbability"></param>
        /// <param name="allParents"></param>
        /// <returns></returns>
        protected virtual IList<IChromosome> PerformCrossPopulation(ISpeciedPopulation population, ICrossover crossover, float crossoverProbability, IList<IChromosome> allParents)
        {
            // (PUZZLE) How to create lookup when fitness is how equality is determined?
            var parentsLookup = allParents.ToLookup(
                (parent) => population.Species.FirstOrDefault(
                    (specie) => specie.CurrentGeneration.Chromosomes.Any(
                        (chrom) => Object.ReferenceEquals(chrom, parent))));

            var toReturn = new List<IChromosome>();
            var newSpecieSizes = CalcNewSpecieSizes(population);
            foreach (var kv in newSpecieSizes)
            {
                Specie specie = kv.Key;
                int numChildren = kv.Value;
                int nCross = (int)Math.Round(numChildren * crossoverProbability);
                var curParents = parentsLookup[specie].ToList();
                // Copy the best chromosome directly
                if (numChildren > 0)
                    toReturn.Add(specie.BestChromosome);
                // Normal crossover for the specie
                toReturn.AddRange(PerformCrossSpecie(population, specie, crossover, nCross, allParents, parentsLookup));
                // For the rest, copy over random parents
                // Include -1 to leave space for BestChromosome to be directly copied
                toReturn.AddRange(GetRandomParents(curParents, numChildren - nCross - 1));
            }

            return toReturn;
        }

        /// <summary>
        /// Enumerates through the offspring created from the specie.
        /// </summary>
        /// <param name="population"></param>
        /// <param name="specie"></param>
        /// <param name="crossover"></param>
        /// <param name="childrenCount"></param>
        /// <param name="allParents"></param>
        /// <param name="parentsLookup"></param>
        /// <returns></returns>
        protected virtual IEnumerable<IChromosome> PerformCrossSpecie(
            ISpeciedPopulation population,
            Specie specie,
            ICrossover crossover, 
            int childrenCount,
            IList<IChromosome> allParents,
            ILookup<Specie, IChromosome> parentsLookup)
        {
            if (parentsLookup[specie].Count() < crossover.ParentsNumber)
                throw new InvalidOperationException("Not enough specie parents available for this crossover.");

            if (childrenCount <= 0)
                yield break;
            
            var curParents = new List<IChromosome>(crossover.ParentsNumber);
            int pInd = 0;
            for (int i = 0; i < childrenCount; i += crossover.ChildrenNumber)
            {
                IList<IChromosome> kids;

                // Interspecie crossover
                if (RandomizationProvider.Current.GetDouble() <= InterspecieMatingRate)
                {
                    kids = PerformInterspecieCross(crossover, allParents);
                }
                // Crossover within specie
                else
                {
                    List<IChromosome> specieParents = parentsLookup[specie].ToList();
                    // Choose parents, place into curParents list
                    curParents.Clear();
                    while (curParents.Count < crossover.ParentsNumber)
                    {
                        curParents.Add(specieParents[pInd++]);
                        // Wrap around and re-shuffle
                        if (pInd >= specieParents.Count)
                        {
                            pInd = 0;
                            specieParents = specieParents.Shuffle(RandomizationProvider.Current).ToList();
                        }
                    }
                    kids = crossover.Cross(curParents);
                }

                // Return children
                for (int k = 0; k < kids.Count; k++)
                    if (i + k < childrenCount)
                        yield return kids[k];
            }
        }

        /// <summary>
        /// Returns a dictionary mapping each specie to its number of offspring.
        /// </summary>
        /// <param name="population"></param>
        /// <returns></returns>
        protected virtual Dictionary<Specie, int> CalcNewSpecieSizes(ISpeciedPopulation population)
        {
            var species = population.Species;
            var toReturn = new Dictionary<Specie, int>(species.Count);
            var avgFits = species.Select(s => GetAverageFitness(s)).ToList();
            double totalAvgFit = avgFits.Sum();

            int totalChildren = 0; // Keep track for later
            for (int i = 0; i < species.Count; i++)
            {
                int newSize = GetNewSpecieSize(species[i], avgFits[i], totalAvgFit, population);
                toReturn.Add(species[i], newSize);
                totalChildren += newSize;
            }

            if (totalChildren == 0)
                return new Dictionary<Specie, int>();

            // Expand to minsize
            for (; totalChildren < population.MinSize; totalChildren++)
            {
                var minkv = GetMinNonZeroValue(toReturn);
                if (minkv.Value == int.MaxValue)
                    throw new InvalidOperationException("All species were set to size zero.");
                toReturn[minkv.Key]++;
            }

            // Trim to maxsize
            for(; totalChildren > population.MaxSize; totalChildren--)
            {
                var maxkv = GetMaxValue(toReturn);
                if (maxkv.Value <= population.MinSpecieSize)
                    throw new InvalidOperationException("MinSpecieSize is larger than expected.");
                toReturn[maxkv.Key]--;
            }

            return toReturn;
        }

        /// <summary>
        /// Returns true if the specie should reproduce, false if it
        /// should be removed from the population.
        /// </summary>
        protected virtual bool SpecieShouldReproduce(Specie specie)
        {
            // Let newer generations reproduce
            if (specie.GenerationsNumber < 10)
                return true;
           
            // Remove species which have not improved
            // TODO: Make agnostic to size of generations list
            double oldFitness = specie.Generations[specie.Generations.Count - 10].BestChromosome.Fitness.Value;
            if (specie.BestChromosome.Fitness.Value <= oldFitness)
                return false;

            return true;
        }

        /// <summary>
        /// Performs a crossover by choosing parents uniformly from the list of parents.
        /// </summary>
        protected IList<IChromosome> PerformInterspecieCross(ICrossover crossover, IList<IChromosome> allParents)
        {
            var ints = RandomizationProvider.Current.GetUniqueInts(crossover.ParentsNumber, 0, allParents.Count);
            var curParents = new IChromosome[crossover.ParentsNumber];
            for (int i = 0; i < crossover.ParentsNumber; i++)
                curParents[i] = allParents[ints[i]];
            return crossover.Cross(curParents);
        }
        #endregion

        #region Helpers
        private double GetAverageFitness(Specie specie) =>
            specie.CurrentGeneration.Chromosomes.Average(c => c.Fitness.Value);

        private KeyValuePair<Specie, int> GetMaxValue(Dictionary<Specie, int> sizes)
        {
            var maxkv = new KeyValuePair<Specie, int>();
            foreach (var kv in sizes)
            {
                if (kv.Value > maxkv.Value)
                    maxkv = kv;
            }
            return maxkv;
        }

        private KeyValuePair<Specie, int> GetMinNonZeroValue(Dictionary<Specie, int> sizes)
        {
            var minkv = new KeyValuePair<Specie, int>(null, int.MaxValue);
            foreach (var kv in sizes)
            {
                if (kv.Value != 0 && kv.Value < minkv.Value)
                    minkv = kv;
            }
            return minkv;
        }

        private IEnumerable<IChromosome> GetRandomParents(IList<IChromosome> parents, int count)
        {
            for (int i = 0; i < count; i++)
            {
                int choice = RandomizationProvider.Current.GetInt(0, parents.Count);
                yield return parents[choice];
            }
        }

        /// <summary>
        /// Calculates the new size of a specie given its average fitness, the
        /// sum of all average fitnesses, and the population.
        /// </summary>
        private int GetNewSpecieSize(Specie specie, double avgFit, double totalAvgFit, ISpeciedPopulation population)
        {
            if (!SpecieShouldReproduce(specie))
                return 0;
            double ratio = avgFit / totalAvgFit;
            int newSize = (int)Math.Ceiling(ratio * population.MinSize);
            return (newSize < population.MinSpecieSize) ? 0 : newSize;
        }
        #endregion
    }
}
