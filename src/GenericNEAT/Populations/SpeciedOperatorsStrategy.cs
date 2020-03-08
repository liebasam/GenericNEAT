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
        public IList<IChromosome> Cross(IPopulation population, ICrossover crossover, float crossoverProbability, IList<IChromosome> parents)
        {
            var pop = population as SpeciedPopulation;
            if (pop is null)
                throw new InvalidOperationException();
            return PerformCross(pop, crossover, crossoverProbability, parents);
        }

        public void Mutate(IMutation mutation, float mutationProbability, IList<IChromosome> chromosomes)
        {
            for (int i = 0; i < chromosomes.Count; i++)
                mutation.Mutate(chromosomes[i], mutationProbability);
        }

        #region Methods
        protected virtual IList<IChromosome> PerformCross(SpeciedPopulation population, ICrossover crossover, float crossoverProbability, IList<IChromosome> parents)
        {
            var toReturn = new List<IChromosome>();
            var sizes = CalcNewSpecieSizes(population);
            foreach (var kv in sizes)
            {
                Specie specie = kv.Key;
                int newSize = kv.Value;
                int nCross = (int)Math.Round(newSize * crossoverProbability);
                var curParents = parents.Where(c => specie.CurrentGeneration.Chromosomes.Contains(c)).ToList();
                toReturn.AddRange(PerformCross(specie, crossover, nCross, curParents));
                // Include -1 to leave space for BestChromosome to be directly copied
                toReturn.AddRange(GetRandomParents(curParents, newSize - nCross - 1));
            }

            return toReturn;
        }

        protected virtual IEnumerable<IChromosome> PerformCross(Specie specie, ICrossover crossover, int childrenCount, IList<IChromosome> parents)
        {
            if (parents.Count < crossover.ParentsNumber)
                throw new InvalidOperationException();

            var curParents = new List<IChromosome>(crossover.ParentsNumber);
            int pInd = 0;
            for (int i = 0; i < childrenCount; i += crossover.ChildrenNumber)
            {
                // Group current parents
                curParents.Clear();
                while (curParents.Count < crossover.ParentsNumber)
                {
                    curParents.Add(parents[pInd++]);
                    // Wrap around
                    if (pInd >= parents.Count)
                    {
                        pInd = 0;
                        parents = parents.Shuffle(RandomizationProvider.Current).ToList();
                    }
                }

                var kids = crossover.Cross(curParents);
                for (int k = 0; k < kids.Count; k++)
                    if (i + k < childrenCount)
                        yield return kids[k];
            }
        }

        protected virtual Dictionary<Specie, int> CalcNewSpecieSizes(SpeciedPopulation population)
        {
            // Remove species which should not reproduce
            var species = population.Species;
            for (int i = 0; i < species.Count; i++)
                if (!SpecieShouldReproduce(species[i]))
                {
                    species.RemoveAt(i);
                    i--;
                }

            // Calculate new sizes
            var toReturn = new Dictionary<Specie, int>(species.Count);
            var avgFits = species.Select(s => GetAverageFitness(s)).ToList();
            double totalAvgFit = avgFits.Sum();
            int totalChildren = 0;
            for (int i = 0; i < species.Count; i++)
            {
                double ratio = avgFits[i] / totalAvgFit;
                int newSize = (int)Math.Ceiling(ratio * population.MinSize);
                newSize = Math.Max(newSize, population.MinSpecieSize);
                toReturn.Add(species[i], newSize);
                totalChildren += newSize;
            }

            // Trim to size
            for(; totalChildren > population.MaxSize; totalChildren--)
            {
                var maxkv = MaxKey(toReturn);
                if (maxkv.Value <= population.MinSpecieSize)
                    throw new InvalidOperationException();
                toReturn[maxkv.Key]--;
            }

            return toReturn;
        }

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
        #endregion

        #region Helpers
        private double GetAverageFitness(Specie specie) =>
            specie.CurrentGeneration.Chromosomes.Average(c => c.Fitness.Value);

        private KeyValuePair<Specie, int> MaxKey(Dictionary<Specie, int> sizes)
        {
            var maxkv = new KeyValuePair<Specie, int>();
            foreach (var kv in sizes)
            {
                if (kv.Value > maxkv.Value)
                    maxkv = kv;
            }
            return maxkv;
        }

        private IEnumerable<IChromosome> GetRandomParents(IList<IChromosome> parents, int count)
        {
            for (int i = 0; i < count; i++)
                yield return parents[RandomizationProvider.Current.GetInt(0, parents.Count)];
        }
        #endregion
    }
}
