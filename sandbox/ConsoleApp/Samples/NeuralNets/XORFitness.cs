using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Randomizations;
using System;

namespace GenericNEAT.Samples.NeuralNets
{
    /// <summary>
    /// The difference between each actual and expected output is
    /// summed to find the error. The error is subtracted from 4
    /// to get the final fitness. Thus, fitness is between 0 and 4.
    /// </summary>
    public class XORFitness : NNetFitnessBase
    {
        readonly float[][] Inputs = new float[][]
        {
            new float[]{0,0 },
            new float[]{0,1 },
            new float[]{1,0 },
            new float[]{1,1 }
        };

        public override double Evaluate(IBlackBox net)
        {
            if (net.InputCount != 2 || net.OutputCount != 1)
                throw new IndexOutOfRangeException();
            var inputs = MutationService.Shuffle(Inputs, RandomizationProvider.Current);
            double error = 0;
            foreach (var ins in inputs)
            {
                net.Reset();
                net.SetInputs(ins);
                net.Activate(3);
                double output = net.GetOutput(0);
                double expected = (ins[0] == ins[1]) ? 0 : 1;
                double diff = output - expected;
                error += (diff >= 0) ? diff : -diff;
            }
            return 4 - error;
        }
    }
}
