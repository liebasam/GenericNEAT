using GeneticSharp.Domain.Chromosomes;
using LiebasamUtils.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GenericNEAT.Samples.NeuralNets
{
    public sealed class NNetChromosome : GraphChromosome
    {
        #region Properties
        /// <summary>
        /// Number of input neurons.
        /// </summary>
        public int InputCount { get; }

        /// <summary>
        /// Number of output neurons.
        /// </summary>
        public int OutputCount { get; }

        /// <summary>
        /// Number of hidden layers.
        /// </summary>
        public int HiddenCount => VertexCount - InputCount - OutputCount;
        
        /// <summary>
        /// Enumerates the input neuron vertices.
        /// </summary>
        public IEnumerable<Vertex<IChromosome>> InputNeurons
        {
            get
            {
                for (uint i = 0; i < InputCount; i++)
                    yield return new Vertex<IChromosome>(i, this[i]);
            }
        }

        /// <summary>
        /// Enumerates the output neuron vertices.
        /// </summary>
        public IEnumerable<Vertex<IChromosome>> OutputNeurons
        {
            get
            {
                for (uint i = (uint)InputCount; i < InputCount + OutputCount; i++)
                    yield return new Vertex<IChromosome>(i, this[i]);
            }
        }

        /// <summary>
        /// Enumerates the hidden neuron vertices.
        /// </summary>
        public IEnumerable<Vertex<IChromosome>> HiddenNeurons
            => Vertices.Where(v => v.ID >= InputCount + OutputCount);
        #endregion

        #region Constructors
        /// <summary>
        /// The chromosome of a fully-connected feedforward neural network.
        /// </summary>
        /// <param name="layerSizes">List of layer sizes. 0th entry is
        /// input size, 1st entry is output size, follwed by hidden layrs.</param>
        public NNetChromosome(params int[] layerSizes)
        {
            if (layerSizes is null)
                throw new ArgumentNullException(nameof(layerSizes));
            if (layerSizes.Length < 2)
                throw new IndexOutOfRangeException();
            for (int i = 0; i < layerSizes.Length; i++)
                if (layerSizes[i] < 1)
                    throw new IndexOutOfRangeException();
            InputCount = layerSizes[0];
            OutputCount = layerSizes[1];
            // Create vertices
            for (uint id = 0; id < layerSizes.Sum(); id++)
                AddVertex(id, GenerateVertex());
            // Fully connect each layer
            for (int layerInd = 0; layerInd < layerSizes.Length - 1; layerInd++)
                foreach (var fromVert in GetNeuronsFromLayer(layerInd, layerSizes))
                    foreach (var toVert in GetNeuronsFromLayer(layerInd + 1, layerSizes))
                        AddEdge(fromVert.ID, toVert.ID, GenerateEdge());
        }

        /// <summary>
        /// Creates a neural network with the specified number of inputs, outputs, 
        /// and hidden neurons. No connections are created.
        /// </summary>
        public NNetChromosome(int numInput, int numOutput, int numHidden)
        {
            if (numInput < 0)
                throw new ArgumentOutOfRangeException(nameof(numInput));
            if (numOutput < 0)
                throw new ArgumentOutOfRangeException(nameof(numOutput));
            if (numHidden < 0)
                throw new ArgumentOutOfRangeException(nameof(numHidden));
            InputCount = numInput;
            OutputCount = numOutput;
            for (uint id = 0; id < numInput + numOutput + numHidden; id++)
                AddVertex(id, GenerateVertex());
        }

        private NNetChromosome(IEnumerable<Vertex<IChromosome>> vertices, IEnumerable<Edge<IChromosome>> edges)
            : base(vertices, edges) { }
        #endregion

        #region Methods
        /// <summary>
        /// Gets the hidden neruons for a particular layer. Inputs on layer 0,
        /// outputs on layer 1, with the rest being hidden sizes.
        /// </summary>
        IEnumerable<Vertex<IChromosome>> GetNeuronsFromLayer(int layer, int[] layerSizes)
        {
            if (layer < 0 || layer >= layerSizes.Length)
                throw new IndexOutOfRangeException();
            uint startInd = (uint)(layerSizes.Take(layer).Sum());
            for (uint i = 0; i < layerSizes[layer]; i++)
                yield return new Vertex<IChromosome>(startInd + i, this[startInd + i]);
        }

        public override IChromosome Clone() => new NNetChromosome(CloneVertices(), CloneEdges());

        public override IChromosome CreateNew() => new NNetChromosome(CreateNewVertices(), CreateNewEdges());

        public override IChromosome GenerateEdge() => new FloatingPointChromosome(-1, 1, 64, 4);

        public override IChromosome GenerateVertex() => new FloatingPointChromosome(-1, 1, 64, 4);
        #endregion
    }
}
