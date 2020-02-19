using GeneticSharp.Domain.Chromosomes;
using LiebasamUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericNEAT.Samples.NeuralNets
{
    public class NNet
    {
        protected readonly NNetChromosome Template;
        public int InputSize { get; }
        public int OutputSize { get; }
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

        /// <summary>
        /// Current outputs of the network.
        /// </summary>
        public float[] CurrentOutputs => Activations
            .Skip(InputSize)
            .Take(OutputSize)
            .ToArray();

        public NNet(NNetChromosome template)
        {
            if (template is null)
                throw new ArgumentNullException();
            InputSize = Template.InputCount;
            OutputSize = Template.OutputCount;
            Activations = new float[template.VertexCount];
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
            Connections = template.VertexIDs.Skip(InputSize).Select(v2 =>
                template.VertexIDs.Select(v1 =>
                {
                    if (template.ContainsEdge(v1, v2))
                        return ToFloatingPoint(template[v1, v2]);
                    else
                        return 0f;
                }).ToArray())
                .ToArray();
        }

        /// <summary>
        /// Transfer function producing a neuron's output.
        /// </summary>
        protected virtual float TransferFunction(float x) => (float)Math.Tanh(x);

        /// <summary>
        /// Sets the inputs to the network.
        /// </summary>
        public void SetInputs(IEnumerable<float> inputs)
        {
            if (inputs.Count() != InputSize)
                throw new IndexOutOfRangeException();
            int i = 0;
            foreach (var input in inputs)
                Activations[i++] = input;
        }

        /// <summary>
        /// Activates the network.
        /// </summary>
        public void Activate()
        {
            FastMath.MatrixMultiply(Activations, Connections, _tempActivations);
            ApplyTransferFunction();
            _tempActivations.CopyTo(Activations, InputSize);
        }

        /// <summary>
        /// Sets the inputs and activates the network.
        /// </summary>
        public void Activate(IEnumerable<float> inputs)
        {
            SetInputs(inputs);
            Activate();
        }

        protected float ToFloatingPoint(IChromosome floatingPointChromosome) =>
            (float)(floatingPointChromosome as FloatingPointChromosome).ToFloatingPoint();

        private void ApplyTransferFunction()
        {
            for (int i = 0; i < _tempActivations.Length; i++)
                _tempActivations[i] = TransferFunction(_tempActivations[i]);
        }
    }
}
