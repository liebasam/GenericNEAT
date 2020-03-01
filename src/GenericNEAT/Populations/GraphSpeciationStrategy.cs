using GenericNEAT.Chromosomes;
using GeneticSharp.Domain.Chromosomes;
using System;

namespace GenericNEAT.Populations
{
    /// <summary>
    /// Generic implementation of <see cref="GraphChromosomeBase"/>.
    /// </summary>
    public sealed class GraphSpeciationStrategy : GraphSpeciationStrategyBase
    {
        #region Fields
        /// <summary>
        /// Function to calculate the distance between two vertices.
        /// </summary>
        public Func<IChromosome, IChromosome, double> VertexDistanceFunc { get; set; }

        /// <summary>
        /// Function to calculate the distance between two edges.
        /// </summary>
        public Func<IChromosome, IChromosome, double> EdgeDistanceFunc { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new GraphSpeciationStrategy with the specified parameters.
        /// </summary>
        /// <param name="vertexDistanceFunction">Function for calculating the
        /// distance between two vertices.</param>
        /// <param name="edgeDistanceFunction">Function for calculating the
        /// distance between two edges.</param>
        /// <param name="vertexWeight">Multiplier for vetex distance.</param>
        /// <param name="edgeWeight">Multiplier for edge distance.</param>
        /// <param name="vertexMismatchWeight">Distance that each mismatched
        /// vertex adds to the calculation.</param>
        /// <param name="edgeMismatchWeight">Distance that each mismatched
        /// edge adds to the caclulation.</param>
        public GraphSpeciationStrategy(double threshold,
            double vertexWeight, double edgeWeight,
            double vertexMismatchWeight, double edgeMismatchWeight,
            Func<IChromosome, IChromosome, double> vertexDistanceFunction,
            Func<IChromosome, IChromosome, double> edgeDistanceFunction) : base(threshold, vertexWeight, edgeWeight, vertexMismatchWeight, edgeMismatchWeight
        {
            if (vertexDistanceFunction is null)
                throw new ArgumentNullException(nameof(vertexDistanceFunction));
            if (edgeDistanceFunction is null)
                throw new ArgumentNullException(nameof(edgeDistanceFunction));
            VertexDistanceFunc = vertexDistanceFunction;
            EdgeDistanceFunc = edgeDistanceFunction;
        }
        #endregion

        #region Methods
        public override double VertexDistance(IChromosome a, IChromosome b) => VertexDistanceFunc(a, b);

        public override double EdgeDistance(IChromosome a, IChromosome b) => EdgeDistanceFunc(a, b);
        #endregion
    }
}
