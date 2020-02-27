using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Populations;
using System;
using System.Collections.Generic;
using System.Text;

namespace GenericNEAT.Populations
{
    public class SpeciedPopulation : IPopulation
    {
        public DateTime CreationDate { get; } = DateTime.Now;

        public IList<Generation> Generations => throw new NotImplementedException();

        public Generation CurrentGeneration => throw new NotImplementedException();

        public int GenerationsNumber => throw new NotImplementedException();

        public int MinSize { get; set; }
        public int MaxSize { get; set; }

        public IChromosome BestChromosome => throw new NotImplementedException();

        public IGenerationStrategy GenerationStrategy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public event EventHandler BestChromosomeChanged;

        public void CreateInitialGeneration()
        {
            throw new NotImplementedException();
        }

        public void CreateNewGeneration(IList<IChromosome> chromosomes)
        {
            throw new NotImplementedException();
        }

        public void EndCurrentGeneration()
        {
            throw new NotImplementedException();
        }
    }
}
