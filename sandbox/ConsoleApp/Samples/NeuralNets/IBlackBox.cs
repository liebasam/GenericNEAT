using System.Collections.Generic;

namespace GenericNEAT.Samples.NeuralNets
{
    /// <summary>
    /// Interface for stateful input-output systems.
    /// </summary>
    public interface IBlackBox
    {
        /// <summary>
        /// Number of inputs to the network.
        /// </summary>
        int InputCount { get; }

        /// <summary>
        /// Number of outputs from the network.
        /// </summary>
        int OutputCount { get; }

        /// <summary>
        /// Sets the value of a particular input.
        /// </summary>
        void SetInput(int index, float value);

        /// <summary>
        /// Sets the inputs to the network.
        /// </summary>
        void SetInputs(IEnumerable<float> inputs);

        /// <summary>
        /// Gets the output at a specific index.
        /// </summary>
        float GetOutput(int i);

        /// <summary>
        /// Gets all the outputs to the network.
        /// </summary>
        float[] GetOutputs();

        /// <summary>
        /// Activates the network for a single time step.
        /// </summary>
        void Activate();

        /// <summary>
        /// Activates the network a certain number of times.
        /// </summary>
        void Activate(int nSteps);

        /// <summary>
        /// Resets the network to its initial state.
        /// </summary>
        void Reset();
    }
}
