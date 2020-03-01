using GenericNEAT.Chromosomes;
using GeneticSharp.Domain.Chromosomes;
using System;

namespace GenericNEAT.Populations
{
    /// <summary>
    /// Base class for speciation strategies applied to a
    /// <see cref="IGraphChromosome"/>.
    /// </summary>
    public abstract class GraphSpeciationStrategyBase : SpeciationStrategyBase
    {
        #region Fields
        /// <summary>
        /// Multiplier for distance between vertices.
        /// </summary>
        public double VertexMatchWeight { get; set; }

        /// <summary>
        /// Multiplier for distance between edges.
        /// </summary>
        public double EdgeMatchWeight { get; set; }

        /// <summary>
        /// Sets <see cref="VertexMatchWeight"/> and <see cref="EdgeMatchWeight"/>.
        /// </summary>
        public double MatchWeight
        {
            set
            {
                VertexMatchWeight = value;
                EdgeMatchWeight = value;
            }
        }

        /// <summary>
        /// Weight that each mismatched vertex carries.
        /// </summary>
        public double VertexMismatchWeight { get; set; }

        /// <summary>
        /// Weight that each mismatched edge carries.
        /// </summary>
        public double EdgeMismatchWeight { get; set; }

        /// <summary>
        /// Sets <see cref="VertexMismatchWeight"/> and <see cref="EdgeMismatchWeight"/>.
        /// </summary>
        public double MismatchWeight
        {
            set
            {
                VertexMismatchWeight = value;
                EdgeMismatchWeight = value;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a speciation strategy for graphs with the
        /// given parameters.
        /// </summary>
        /// <param name="distanceThreshold">Minimum distance
        /// between any two chromosomes to be considered in
        /// the same specie.</param>
        /// <param name="vertexMatchWeight">Multiplier for calculating
        /// average vertex distance.</param>
        /// <param name="edgeMatchWeight">Multiplier for calculating
        /// average edge distance.</param>
        /// <param name="vertexMismatchWeight">Weight of each
        /// mismatched vertex in the distance calculation.</param>
        /// <param name="edgeMismatchWeight">Weight of each
        /// mismatched edge in the distance calculation.</param>
        public GraphSpeciationStrategyBase(double distanceThreshold,
            double vertexMatchWeight, double edgeMatchWeight,
            double vertexMismatchWeight, double edgeMismatchWeight) : base(distanceThreshold)
        {
            VertexMatchWeight = vertexMatchWeight;
            EdgeMatchWeight = edgeMatchWeight;
            VertexMismatchWeight = vertexMismatchWeight;
            EdgeMismatchWeight = edgeMismatchWeight;
        }

        /// <summary>
        /// Creates a speciation strategy for graphs with the
        /// given parameters.
        /// </summary>
        /// <param name="distanceThreshold">Minimum distance
        /// between any two chromosomes to be considered in
        /// the same specie.</param>
        /// <param name="matchWeight">Multiplier for calculating
        /// average distance.</param>
        /// <param name="mismatchWeight">Weight of each mismatched
        /// vertex or edge.</param>
        public GraphSpeciationStrategyBase(double distanceThreshold, double matchWeight, double mismatchWeight) : base(distanceThreshold)
        {
            MatchWeight = matchWeight;
            MismatchWeight = mismatchWeight;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Calculates the distance between two vertex chromosomes.
        /// </summary>
        protected abstract double VertexDistance(IChromosome a, IChromosome b);

        /// <summary>
        /// Calculates the distance between two edge chromosomes.
        /// </summary>
        protected abstract double EdgeDistance(IChromosome a, IChromosome b);

        public override double DistanceBetween(IChromosome a, IChromosome b)
        {
            var g1 = a as IGraphChromosome;
            var g2 = b as IGraphChromosome;
            if (g1 is null || g2 is null)
                throw new InvalidOperationException();

            return TotalVertexDistance(g1, g2) + TotalEdgeDistance(g1, g2);
        }

        /// <summary>
        /// Calculates the total distance between two graphs, only
        /// accounting for vertices.
        /// </summary>
        protected double TotalVertexDistance(IGraphChromosome g1, IGraphChromosome g2)
        {
            double nMismatch = 0;
            double nMatch = 0;
            double totalDist = 0;
            foreach (var vert in g1.Vertices)
            {
                if (g2.ContainsVertex(vert.ID))
                {
                    totalDist += VertexDistance(g2[vert.ID], vert.Value);
                    nMatch++;
                }
                else
                    nMismatch++;
            }
            foreach (var vert in g2.Vertices)
                if (!g1.ContainsVertex(vert.ID))
                    nMismatch++;

            int N = Math.Max(g1.VertexCount, g2.VertexCount);
            return (VertexMismatchWeight * nMismatch / N) + (VertexMatchWeight * totalDist / nMatch);
        }
        
        /// <summary>
        /// Calculates the total distance between two graphs, only
        /// accounting for edges.
        /// </summary>
        /// <param name="g1"></param>
        /// <param name="g2"></param>
        /// <returns></returns>
        protected double TotalEdgeDistance(IGraphChromosome g1, IGraphChromosome g2)
        {
            double totalDist = 0;
            double nMismatch = 0;
            double nMatch = 0;
            foreach (var edge in g1.Edges)
            {
                if (g2.ContainsEdge(edge.IDFrom, edge.IDTo))
                {
                    totalDist += EdgeDistance(g2[edge.IDFrom, edge.IDTo], edge.Value);
                    nMatch++;
                }
                else
                    nMismatch++;
            }
            foreach (var edge in g2.Edges)
                if (!g1.ContainsEdge(edge.IDFrom, edge.IDTo))
                    nMismatch++;

            int N = Math.Max(g1.EdgeCount, g2.EdgeCount);
            return (EdgeMismatchWeight * nMismatch / N) + (EdgeMatchWeight * totalDist / nMatch);
        }
        #endregion
    }
}
