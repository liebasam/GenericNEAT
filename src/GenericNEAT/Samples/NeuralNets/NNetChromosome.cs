using GenericNEAT.Chromosomes;
using GeneticSharp.Domain.Chromosomes;
using LiebasamUtils.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GenericNEAT.Samples.NeuralNets
{
    public class NNetChromosome : GraphChromosomeBase
    {
        #region Fields
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
        public IEnumerable<Vertex<IChromosome>> InputNeurons => GetNeurons(0, InputCount);

        /// <summary>
        /// Enumerates the output neuron vertices.
        /// </summary>
        public IEnumerable<Vertex<IChromosome>> OutputNeurons => GetNeurons(InputCount, OutputCount);

        /// <summary>
        /// Enumerates the hidden neuron vertices.
        /// </summary>
        public IEnumerable<Vertex<IChromosome>> HiddenNeurons => GetNeurons(InputCount + OutputCount, HiddenCount);
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a neural network with the specified number of inputs, outputs, 
        /// and hidden neurons. No connections are created.
        /// </summary>
        public NNetChromosome(int numInput, int numOutput, int numHidden) : base(numInput + numOutput + numHidden)
        {
            if (numInput <= 0)
                throw new ArgumentOutOfRangeException(nameof(numInput));
            if (numOutput <= 0)
                throw new ArgumentOutOfRangeException(nameof(numOutput));
            if (numHidden < 0)
                throw new ArgumentOutOfRangeException(nameof(numHidden));
            InputCount = numInput;
            OutputCount = numOutput;
            for (uint id = 0; id < numInput + numOutput + numHidden; id++)
                AddVertex(id);
        }

        /// <summary>
        /// Creates a neural network with the specified number of inputs and outputs.
        /// No connections are created.
        /// </summary>
        /// <param name="numInput"></param>
        /// <param name="numOutput"></param>
        public NNetChromosome(int numInput, int numOutput) : this(numInput, numOutput, 0) { }

        private NNetChromosome(int numInput, int numOutput, IEnumerable<Vertex<IChromosome>> vertices, IEnumerable<Edge<IChromosome>> edges)
            : base(vertices, edges)
        {
            InputCount = numInput;
            OutputCount = numOutput;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns a <see cref="FloatingPointChromosome"/> between -1 and 1
        /// with 4 digits of precision.
        /// </summary>
        protected IChromosome GetFloat() => new FloatingPointChromosome(-1, 1, 64, 4);

        public override IChromosome Clone() => new NNetChromosome(InputCount, OutputCount, CloneVertices(), CloneEdges());

        public override IChromosome CreateNew() => new NNetChromosome(InputCount, OutputCount, CreateNewVertices(), CreateNewEdges());

        public override IChromosome CreateNewVertex() => GetFloat();

        public override IChromosome CreateNewEdge() => GetFloat();

        IEnumerable<Vertex<IChromosome>> GetNeurons(int from, int count)
        {
            for (uint i = (uint)from; i < from + count; i++)
                yield return new Vertex<IChromosome>(i, this[i]);
        }
        #endregion
    }
}
