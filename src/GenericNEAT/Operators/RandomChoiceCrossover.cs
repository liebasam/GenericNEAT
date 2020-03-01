using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Randomizations;
using System.Collections.Generic;

namespace GenericNEAT.Operators
{
    public class RandomChoiceCrossover : CrossoverBase
    {
        public RandomChoiceCrossover() : base(2, 1) { }

        protected override IList<IChromosome> PerformCross(IList<IChromosome> parents)
        {
            var choice = parents[RandomizationProvider.Current.GetInt(0, parents.Count)];
            return new IChromosome[] { choice.Clone() };
        }
    }
}
