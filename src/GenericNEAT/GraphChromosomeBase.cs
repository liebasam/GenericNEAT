using GeneticSharp.Domain.Chromosomes;
using LiebasamUtils.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GenericNEAT
{
    public abstract class GraphChromosomeBase : Graph<IChromosome, IChromosome>, IChromosome
    {
        public double? Fitness { get; set; }

        public int Length => Vertices.Sum(v => v.Value.Length) + Edges.Sum(e => e.Value.Length);

        #region Constructors
        public GraphChromosomeBase() : base() { }

        public GraphChromosomeBase(int capacity) : base(capacity) { }

        public GraphChromosomeBase(IEnumerable<Vertex<IChromosome>> vertices, IEnumerable<Edge<IChromosome>> edges) 
            : base(vertices, edges) { }
        #endregion

        #region Methods
        public abstract IChromosome CreateNew();

        public abstract IChromosome Clone();

        /// <summary>
        /// Generates a new vertex.
        /// </summary>
        /// <returns></returns>
        public abstract IChromosome GenerateVertex();

        /// <summary>
        /// Generates a new edge.
        /// </summary>
        /// <returns></returns>
        public abstract IChromosome GenerateEdge();

        /// <summary>
        /// Compare fitness.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(IChromosome other) => Fitness.Value.CompareTo(other.Fitness.Value);

        protected IEnumerable<Vertex<IChromosome>> CloneVertices() =>
            Vertices.Select(v => new Vertex<IChromosome>(v.ID, v.Value.Clone()));

        protected IEnumerable<Edge<IChromosome>> CloneEdges() =>
            Edges.Select(e => new Edge<IChromosome>(e.IDFrom, e.IDTo, e.Value.Clone()));

        protected IEnumerable<Vertex<IChromosome>> CreateNewVertices() =>
            Vertices.Select(v => new Vertex<IChromosome>(v.ID, v.Value.CreateNew()));

        protected IEnumerable<Edge<IChromosome>> CreateNewEdges() =>
            Edges.Select(e => new Edge<IChromosome>(e.IDFrom, e.IDTo, e.Value.CreateNew()));
        #endregion

        // Methods which are unsupported due to the nature of graphs.
        #region Unsupported Methods
        public Gene GenerateGene(int geneIndex) => throw new NotSupportedException();

        public Gene GetGene(int index) => throw new NotSupportedException();

        public Gene[] GetGenes() => throw new NotSupportedException();

        public void ReplaceGene(int index, Gene gene) => throw new NotSupportedException();

        public void ReplaceGenes(int startIndex, Gene[] genes) => throw new NotSupportedException();

        public void Resize(int newLength) => throw new NotSupportedException();
        #endregion
    }
}
