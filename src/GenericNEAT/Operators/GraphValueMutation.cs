﻿using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Mutations;
using System;
using System.Linq;

namespace GenericNEAT.Operators
{
    /// <summary>
    /// Mutates the vertices and edges of a <see cref="GraphChromosomeBase"/>.
    /// </summary>
    public class GraphValueMutation : IMutation
    {
        #region Properties
        /// <summary>
        /// Mutation called on each vertex.
        /// </summary>
        public IMutation VertexMutation { get; set; }

        /// <summary>
        /// Probability passed to <see cref="VertexMutation"/>.
        /// </summary>
        public float? VertexProbability { get; set; } = null;

        /// <summary>
        /// Mutation called on each edge.
        /// </summary>
        public IMutation EdgeMutation { get; set; }

        /// <summary>
        /// Probability passed to <see cref="EdgeMutation"/>.
        /// </summary>
        public float? EdgeProbability { get; set; } = null;

        public bool IsOrdered => true;
        #endregion

        #region Constructors
        /// <summary>
        /// Create a <see cref="GraphChromosomeBase"/> mutation which calls a
        /// separate mutation on each vertex and each edge. The probability
        /// for <see cref="GraphValueMutation"/> is passed to both vertex
        /// and edge mutations.
        /// </summary>
        public GraphValueMutation(IMutation vertexMutation, IMutation edgeMutation)
        {
            if (vertexMutation is null)
                throw new ArgumentNullException(nameof(vertexMutation));
            if (edgeMutation is null)
                throw new ArgumentNullException(nameof(edgeMutation));
            VertexMutation = vertexMutation;
            EdgeMutation = edgeMutation;
        }

        /// <summary>
        /// Create a <see cref="GraphChromosomeBase"/> mutation which calls a
        /// separate mutation on each vertex and each edge. The probability
        /// passed to each mutation is specified.
        /// </summary>
        public GraphValueMutation(IMutation vertexMutation, float vertexProbability,
            IMutation edgeMutation, float edgeProbability)
        {
            if (vertexMutation is null)
                throw new ArgumentNullException(nameof(vertexMutation));
            if (edgeMutation is null)
                throw new ArgumentNullException(nameof(edgeMutation));
            VertexMutation = vertexMutation;
            EdgeMutation = edgeMutation;
            VertexProbability = vertexProbability;
            EdgeProbability = edgeProbability;
        }
        #endregion

        public void Mutate(IChromosome chromosome, float probability)
        {
            if (!(chromosome is GraphChromosomeBase graph))
                throw new InvalidOperationException();

            float prob = VertexProbability.HasValue ? VertexProbability.Value : probability;
            foreach (var vert in graph.Vertices.ToArray())
            {
                VertexMutation.Mutate(vert.Value, prob);
                graph[vert.ID] = vert.Value;
            }

            prob = EdgeProbability.HasValue ? EdgeProbability.Value : probability;
            foreach (var edge in graph.Edges.ToArray())
            {
                EdgeMutation.Mutate(edge.Value, prob);
                graph[edge.IDFrom, edge.IDTo] = edge.Value;
            }
        }
    }
}
