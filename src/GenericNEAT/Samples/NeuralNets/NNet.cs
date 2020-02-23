using GeneticSharp.Domain.Chromosomes;
using LiebasamUtils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GenericNEAT.Samples.NeuralNets
{
    public class NNet
    {
        #region Fields
        protected readonly NNetChromosome Template;
        public int InputCount { get; }
        public int OutputCount { get; }

        /// <summary>
        /// Activation for inputs, outputs, and hidden.
        /// </summary>
        protected readonly float[] Activations;
        private readonly float[] _tempActivations;

        /// <summary>
        /// Bias for outputs and hidden.
        /// </summary>
        protected readonly float[] Bias;

        /// <summary>
        /// Connections, where [i][j] corresponds to the
        /// weight of the connection going from j to i.
        /// </summary>
        protected readonly float[][] Connections;
        #endregion

        #region Constructors
        public NNet(NNetChromosome template)
        {
            if (template is null)
                throw new ArgumentNullException();
            Template = template;
            InputCount = Template.InputCount;
            OutputCount = Template.OutputCount;
            Activations = new float[template.VertexCount];
            _tempActivations = new float[template.VertexCount - template.InputCount];
            var indexToID = new Dictionary<uint, int>(template.VertexCount);
            int curIndex = 0;
            Bias = template.OutputNeurons
                .Concat(template.HiddenNeurons)
                .Select(v =>
                {
                    indexToID.Add(v.ID, curIndex++);
                    return ToFloatingPoint(v.Value);
                })
                .ToArray();
            Connections = template.VertexIDs.Skip(InputCount).Select(v2 =>
                template.VertexIDs.Select(v1 =>
                {
                    if (template.ContainsEdge(v1, v2))
                        return ToFloatingPoint(template[v1, v2]);
                    else
                        return 0f;
                }).ToArray())
                .ToArray();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Transfer function producing a neuron's output.
        /// </summary>
        public virtual float TransferFunction(float x) => (float)(Math.Exp(x) / (1 + Math.Exp(x)));

        /// <summary>
        /// Sets the input of a particular input neuron.
        /// </summary>
        public void SetInput(int index, float value)
        {
            if (index < 0 || index >= InputCount)
                throw new IndexOutOfRangeException();
            Activations[index] = value;
        }

        /// <summary>
        /// Sets the inputs to the network.
        /// </summary>
        public void SetInputs(IEnumerable<float> inputs)
        {
            if (inputs.Count() != InputCount)
                throw new IndexOutOfRangeException();
            int i = 0;
            foreach (var input in inputs)
                Activations[i++] = input;
        }

        /// <summary>
        /// Gets the output at a specific index.
        /// </summary>
        public float GetOutput(int i)
        {
            if (i < 0 || i >= OutputCount)
                throw new IndexOutOfRangeException();
            return Activations[i + InputCount];
        }

        /// <summary>
        /// Gets all the outputs to the network.
        /// </summary>
        /// <returns></returns>
        public float[] GetOutputs() => Activations
            .Skip(InputCount)
            .Take(OutputCount)
            .ToArray();

        /// <summary>
        /// Gets the internal state of the network.
        /// </summary>
        public float[] GetState() => (float[])Activations.Clone();

        /// <summary>
        /// Sets the internal state of the network.
        /// </summary>
        public void SetState(float[] state)
        {
            if (state is null)
                throw new ArgumentNullException(nameof(state));
            if (state.Length != Activations.Length)
                throw new IndexOutOfRangeException();
            state.CopyTo(Activations, 0);
        }

        /// <summary>
        /// Activates the network.
        /// </summary>
        public void Activate()
        {
            FastMath.MatrixMultiply(Activations, Connections, _tempActivations);
            FastMath.Add(Bias, _tempActivations, _tempActivations);
            for (int i = 0; i < _tempActivations.Length; i++)
                _tempActivations[i] = TransferFunction(_tempActivations[i]);
            _tempActivations.CopyTo(Activations, InputCount);
        }

        /// <summary>
        /// Sets the inputs and activates the network.
        /// </summary>
        public void Activate(IEnumerable<float> inputs)
        {
            SetInputs(inputs);
            Activate();
        }

        /// <summary>
        /// Resets the network to its initial state.
        /// </summary>
        public void Reset()
        {
            Array.Clear(Activations, 0, Activations.Length);
        }

        /// <summary>
        /// Gets the weight for particular connection.
        /// </summary>
        /// <param name="iFrom">Index of the source.</param>
        /// <param name="iTo">Index of the target.</param>
        public float GetWeight(int iFrom, int iTo)
        {
            if (iTo < InputCount)
                return 0f;
            return Connections[iTo - InputCount][iFrom];
        }

        /// <summary>
        /// Gets the bias for a particular neuron.
        /// </summary>
        public float GetBias(int i)
        {
            if (i < InputCount)
                return 0;
            return Bias[i - InputCount];
        }

        protected float ToFloatingPoint(IChromosome floatingPointChromosome) =>
            (float)(floatingPointChromosome as FloatingPointChromosome).ToFloatingPoints()[0];
        #endregion
    }
}
