using GeneticSharp.Domain.Chromosomes;
using LiebasamUtils.Collections;
using System;
using System.Collections.Generic;
using System.Text;

namespace GenericNEAT.Chromosomes
{
    public interface IGraphChromosome : IGraph<IChromosome, IChromosome>, IChromosome
    {

    }
}
