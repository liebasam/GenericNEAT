using System;
using System.Collections.Generic;
using System.Linq;

namespace GenericNEAT.Samples.NeuralNets.Tests
{
    internal abstract class XORImplBase : IBlackBox
    {
        protected float[] Inputs = new float[2];

        public int InputCount => 2;
        public int OutputCount => 1;
        public void Activate() { }
        public void Activate(int nSteps) { }
        public abstract float GetOutput(int index);
        public float[] GetOutputs() => new float[] { GetOutput(0) };
        public void Reset() => Array.Clear(Inputs, 0, 2);
        public void SetInput(int index, float value) => Inputs[index] = value;
        public void SetInputs(IEnumerable<float> inputs) => Inputs = inputs.ToArray();
    }

    internal class XORCorrect : XORImplBase
    {
        public override float GetOutput(int index)
        {
            if (Inputs[0] == Inputs[1])
                return 0;
            else
                return 1;
        }
    }

    internal class XORWrong : XORImplBase
    {
        public override float GetOutput(int index)
        {
            if (Inputs[0] == Inputs[1])
                return 1;
            else
                return 0;
        }
    }

    internal class XORHalfRight : XORImplBase
    {
        public override float GetOutput(int index) => 1;
    }
}
