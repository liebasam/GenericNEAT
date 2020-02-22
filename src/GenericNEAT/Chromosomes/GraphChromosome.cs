using GeneticSharp.Domain.Chromosomes;
using LiebasamUtils.Collections;
using System;
using System.Collections.Generic;

namespace GenericNEAT.Chromosomes
{
    public class GraphChromosome : GraphChromosomeBase
    {
        readonly IChromosome VertexTemplate, EdgeTemplate;

        /// <summary>
        /// Creates a GraphChromosome with the given vertex and edge templates.
        /// </summary>
        /// <param name="vertexTemplate">Used to create new vertices.</param>
        /// <param name="edgeTemplate">Used to create new edges.</param>
        public GraphChromosome(IChromosome vertexTemplate, IChromosome edgeTemplate) : base()
        {
            if (vertexTemplate is null)
                throw new ArgumentNullException(nameof(vertexTemplate));
            if (edgeTemplate is null)
                throw new ArgumentNullException(nameof(edgeTemplate));
            VertexTemplate = vertexTemplate;
            EdgeTemplate = edgeTemplate;
        }

        /// <summary>
        /// Creates a GraphChromosome with the given vertex and edge templates
        /// and initial vertex capacity.
        /// </summary>
        /// <param name="vertexTemplate">Used to create new vertices.</param>
        /// <param name="edgeTemplate">Used to create new edges.</param>
        /// <param name="capacity">Initial vertex capacity.</param>
        public GraphChromosome(IChromosome vertexTemplate, IChromosome edgeTemplate, int capacity) : base(capacity)
        {
            if (vertexTemplate is null)
                throw new ArgumentNullException(nameof(vertexTemplate));
            if (edgeTemplate is null)
                throw new ArgumentNullException(nameof(edgeTemplate));
            VertexTemplate = vertexTemplate;
            EdgeTemplate = edgeTemplate;
        }

        internal IChromosome GenerateEdge()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a GraphChromosome with the given vertex and edge templates
        /// and initial vertices and edges.
        /// </summary>
        /// <param name="vertexTemplate">Used to create new vertices.</param>
        /// <param name="edgeTemplate">Used to create new edges.</param>
        /// <param name="vertices">The vertices.</param>
        /// <param name="edges">The edges.</param>
        public GraphChromosome(
            IChromosome vertexTemplate, IChromosome edgeTemplate,
            IEnumerable<Vertex<IChromosome>> vertices, IEnumerable<Edge<IChromosome>> edges) : base(vertices, edges)
        {
            if (vertexTemplate is null)
                throw new ArgumentNullException(nameof(vertexTemplate));
            if (edgeTemplate is null)
                throw new ArgumentNullException(nameof(edgeTemplate));
            VertexTemplate = vertexTemplate;
            EdgeTemplate = edgeTemplate;
        }

        public override IChromosome Clone() => 
            new GraphChromosome(VertexTemplate, EdgeTemplate, CloneVertices(), CloneEdges());

        public override IChromosome CreateNew() => 
            new GraphChromosome(VertexTemplate, EdgeTemplate, CreateNewVertices(), CreateNewEdges());

        public override IChromosome CreateNewEdge() => EdgeTemplate.CreateNew();

        public override IChromosome CreateNewVertex() => VertexTemplate.CreateNew();
    }
}
