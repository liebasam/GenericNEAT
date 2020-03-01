using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Populations;
using System.Collections.Generic;

namespace GenericNEAT.Populations
{
    /// <summary>
    /// Collection of items with a centroid and unique ID.
    /// </summary>
    public class Specie : Population
    {
        /// <summary>
        /// The ID of the specie.
        /// </summary>
        public uint ID { get; }

        /// <summary>
        /// The defining member of the specie.
        /// </summary>
        public IChromosome Centroid { get; }

        /// <summary>
        /// Creates a new specie.
        /// </summary>
        public Specie(int minSize, int maxSize, uint id, IChromosome centroid) : base(minSize, maxSize, centroid)
        {
            ID = id;
            Centroid = centroid;
        }

        public override void CreateInitialGeneration()
        {
            var tmp = MinSize;
            MinSize = MaxSize;
            base.CreateInitialGeneration();
            MinSize = tmp;
        }
    }
}
