using GeneticSharp.Domain.Chromosomes;
using LiebasamUtils.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GenericNEAT.Chromosomes
{
    public abstract class GraphChromosomeBase : Graph<IChromosome, IChromosome>, IGraphChromosome
    {
        #region Fields
        public double? Fitness { get; set; }

        public int Length => Vertices.Sum(v => v.Value.Length) + Edges.Sum(e => e.Value.Length);
        #endregion

        #region Constructors
        /// <summary>
        /// Create an empty graph chromosome.
        /// </summary>
        public GraphChromosomeBase() : base() { }

        /// <summary>
        /// Create an empty graph chromosome with the initial
        /// vertex capacity.
        /// </summary>
        /// <param name="capacity">Initial vertex capacity.</param>
        public GraphChromosomeBase(int capacity) : base(capacity) { }

        /// <summary>
        /// Create a graph with the specified vertices and edges.
        /// </summary>
        /// <param name="vertices">The vertices.</param>
        /// <param name="edges">The edges.</param>
        public GraphChromosomeBase(IEnumerable<Vertex<IChromosome>> vertices, IEnumerable<Edge<IChromosome>> edges)
            : base(vertices, edges) { }
        #endregion

        #region Methods
        public abstract IChromosome Clone();

        public abstract IChromosome CreateNew();

        public abstract IChromosome CreateNewVertex();

        public abstract IChromosome CreateNewEdge();

        /// <summary>
        /// Adds a new vertex to the graph.
        /// </summary>
        public void AddVertex(uint id) => AddVertex(id, CreateNewVertex());

        /// <summary>
        /// Adds a new edge to the graph.
        /// </summary>
        public void AddEdge(uint idFrom, uint idTo) => AddEdge(idFrom, idTo, CreateNewEdge());

        /// <summary>
        /// Calls <see cref="IChromosome.Clone"/> on each vertex.
        /// </summary>
        /// <returns></returns>
        protected IEnumerable<Vertex<IChromosome>> CloneVertices() =>
            Vertices.Select(v => new Vertex<IChromosome>(v.ID, v.Value.Clone()));

        /// <summary>
        /// Calls <see cref="IChromosome.Clone"/> on each edge.
        /// </summary>
        /// <returns></returns>
        protected IEnumerable<Edge<IChromosome>> CloneEdges() =>
            Edges.Select(e => new Edge<IChromosome>(e.IDFrom, e.IDTo, e.Value.Clone()));

        /// <summary>
        /// Calls <see cref="IChromosome.CreateNew"/> on each vertex.
        /// </summary>
        /// <returns></returns>
        protected IEnumerable<Vertex<IChromosome>> CreateNewVertices() =>
            Vertices.Select(v => new Vertex<IChromosome>(v.ID, v.Value.CreateNew()));

        /// <summary>
        /// Calls <see cref="IChromosome.CreateNew"/> on each edge.
        /// </summary>
        /// <returns></returns>
        protected IEnumerable<Edge<IChromosome>> CreateNewEdges() =>
            Edges.Select(e => new Edge<IChromosome>(e.IDFrom, e.IDTo, e.Value.CreateNew()));

        /// <summary>
        /// Converts each vertex to the given type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected IEnumerable<Vertex<T>> ConvertVertices<T>() =>
            Vertices.Select(v => new Vertex<T>(v.ID, (T)v.Value));

        /// <summary>
        /// Converts each edge to the given type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected IEnumerable<Edge<T>> ConvertEdges<T>() =>
            Edges.Select(e => new Edge<T>(e.IDFrom, e.IDTo, (T)e.Value));
        
        /// <summary>
        /// Compares the fitness of two chromosomes.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(IChromosome other)
        {
            if (other is null)
                return -1;
            if (Fitness == other.Fitness)
                return 0;
            return Fitness > other.Fitness ? 1 : -1;
        }

        public virtual Gene GenerateGene(int geneIndex) => throw new NotImplementedException();

        public virtual Gene GetGene(int index) => throw new NotImplementedException();

        public virtual Gene[] GetGenes() => new Gene[0]; //throw new NotImplementedException();

        public virtual void ReplaceGene(int index, Gene gene) => throw new NotImplementedException();

        public virtual void ReplaceGenes(int startIndex, Gene[] genes) => throw new NotImplementedException();

        public virtual void Resize(int newLength) => throw new NotImplementedException();
        #endregion
    }
}
