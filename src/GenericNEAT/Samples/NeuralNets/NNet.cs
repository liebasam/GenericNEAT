using GeneticSharp.Domain.Chromosomes;
using LiebasamUtils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GenericNEAT.Samples.NeuralNets
{
    public class NNet : IBlackBox
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
        public virtual void ApplyTransferFunction(float[] arr) =>
            FastMath.Max(arr, 0, arr);

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

        public void SetInput(int index, float value)
        {
            if (index < 0 || index >= InputCount)
                throw new IndexOutOfRangeException();
            Activations[index] = value;
        }

        public void SetInputs(IEnumerable<float> inputs)
        {
            if (inputs.Count() != InputCount)
                throw new IndexOutOfRangeException();
            int i = 0;
            foreach (var input in inputs)
                Activations[i++] = input;
        }

        public float GetOutput(int i)
        {
            if (i < 0 || i >= OutputCount)
                throw new IndexOutOfRangeException();
            return Activations[i + InputCount];
        }

        public float[] GetOutputs() => Activations
            .Skip(InputCount)
            .Take(OutputCount)
            .ToArray();

        public void Activate()
        {
            FastMath.MatrixMultiply(Activations, Connections, _tempActivations);
            FastMath.Add(Bias, _tempActivations, _tempActivations);
            ApplyTransferFunction(_tempActivations);
            _tempActivations.CopyTo(Activations, InputCount);
        }

        public void Activate(int nSteps)
        {
            for (int i = 0; i < nSteps; i++)
                Activate();
        }
        
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
