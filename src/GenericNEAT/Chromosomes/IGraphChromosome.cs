using GeneticSharp.Domain.Chromosomes;
using LiebasamUtils.Collections;

namespace GenericNEAT.Chromosomes
{
    public interface IGraphChromosome : IGraph<IChromosome, IChromosome>, IChromosome
    {
        /// <summary>
        /// Returns a new vertex.
        /// </summary>
        IChromosome CreateNewVertex();

        /// <summary>
        /// Returns a new edge.
        /// </summary>
        IChromosome CreateNewEdge();
    }
}
