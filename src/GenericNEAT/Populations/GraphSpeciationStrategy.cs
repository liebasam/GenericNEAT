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
        /// <param name="vertexMatchWeight">Multiplier for vetex distance.</param>
        /// <param name="edgeMatchWeight">Multiplier for edge distance.</param>
        /// <param name="vertexMismatchWeight">Distance that each mismatched
        /// vertex adds to the calculation.</param>
        /// <param name="edgeMismatchWeight">Distance that each mismatched
        /// edge adds to the caclulation.</param>
        public GraphSpeciationStrategy(double threshold,
            double vertexMatchWeight, double edgeMatchWeight,
            double vertexMismatchWeight, double edgeMismatchWeight,
            Func<IChromosome, IChromosome, double> vertexDistanceFunction,
            Func<IChromosome, IChromosome, double> edgeDistanceFunction) : base(threshold, vertexMatchWeight, edgeMatchWeight, vertexMismatchWeight, edgeMismatchWeight)
        {
            if (vertexDistanceFunction is null)
                throw new ArgumentNullException(nameof(vertexDistanceFunction));
            if (edgeDistanceFunction is null)
                throw new ArgumentNullException(nameof(edgeDistanceFunction));
            VertexDistanceFunc = vertexDistanceFunction;
            EdgeDistanceFunc = edgeDistanceFunction;
        }

        /// <summary>
        /// Creates a new GraphSpeciationStrategy with the specified parameters.
        /// </summary>
        /// <param name="vertexDistanceFunction">Function for calculating the
        /// distance between two vertices.</param>
        /// <param name="edgeDistanceFunction">Function for calculating the
        /// distance between two edges.</param>
        /// <param name="matchWeight">Multiplier for matching vertices and edges.</param>
        /// <param name="mismatchWeight">Weight that each mismatched vertex
        /// or edge contributes to calculating distance.</param>
        public GraphSpeciationStrategy(double threshold,
            double matchWeight, double mismatchWeight,
            Func<IChromosome, IChromosome, double> vertexDistanceFunction,
            Func<IChromosome, IChromosome, double> edgeDistanceFunction)
            : this(threshold, matchWeight, matchWeight, mismatchWeight, mismatchWeight, vertexDistanceFunction, edgeDistanceFunction) { }
        #endregion

        #region Methods
        protected override double VertexDistance(IChromosome a, IChromosome b) => VertexDistanceFunc(a, b);

        protected override double EdgeDistance(IChromosome a, IChromosome b) => EdgeDistanceFunc(a, b);
        #endregion
    }
}
