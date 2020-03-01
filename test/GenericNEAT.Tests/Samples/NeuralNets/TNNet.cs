using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericNEAT.Samples.NeuralNets.Tests
{
    [TestClass]
    public class TNNet
    {
        [TestClass]
        public class Constructors
        {
            [TestMethod]
            public void Success()
            {
                var template = new NNetChromosome(1, 2, 1);
                var net = new NNet(template);
                Assert.AreEqual(template.InputCount, net.InputCount);
                Assert.AreEqual(template.OutputCount, net.OutputCount);
            }

            [TestMethod]
            public void Failure()
            {
                Assert.ThrowsException<ArgumentNullException>(
                    () => new NNet(null));
            }
        }

        [TestClass]
        public class Methods
        {
            static NNetChromosome Template;
            NNet Net;

            [ClassInitialize]
            public static void ClassInit(TestContext tc)
            {
                Template = new NNetChromosome(1, 1, 2);
                Template.AddEdge(0, 2); Template.AddEdge(2, 1);
                Template.AddEdge(0, 3); Template.AddEdge(3, 1);
            }

            [TestInitialize]
            public void TestInit()
            {
                Net = new NNet(Template);
            }

            [TestMethod]
            public void GetWeightBias()
            {
                Assert.AreEqual(0, Net.GetBias(0), float.Epsilon);
                Assert.AreNotEqual(0, Net.GetBias(1), float.Epsilon);
                Assert.AreNotEqual(0, Net.GetBias(2), float.Epsilon);
                Assert.AreNotEqual(0, Net.GetBias(3), float.Epsilon);


                Assert.AreEqual(0, Net.GetWeight(0, 0), float.Epsilon);
                Assert.AreEqual(0, Net.GetWeight(0, 1), float.Epsilon);
                Assert.AreNotEqual(0, Net.GetWeight(0, 2), float.Epsilon);
                Assert.AreNotEqual(0, Net.GetWeight(0, 3), float.Epsilon);

                Assert.AreEqual(0, Net.GetWeight(1, 0), float.Epsilon);
                Assert.AreEqual(0, Net.GetWeight(1, 1), float.Epsilon);
                Assert.AreEqual(0, Net.GetWeight(1, 2), float.Epsilon);
                Assert.AreEqual(0, Net.GetWeight(1, 3), float.Epsilon);

                Assert.AreEqual(0, Net.GetWeight(2, 0), float.Epsilon);
                Assert.AreNotEqual(0, Net.GetWeight(2, 1), float.Epsilon);
                Assert.AreEqual(0, Net.GetWeight(2, 2), float.Epsilon);
                Assert.AreEqual(0, Net.GetWeight(2, 3), float.Epsilon);

                Assert.AreEqual(0, Net.GetWeight(3, 0), float.Epsilon);
                Assert.AreNotEqual(0, Net.GetWeight(3, 1), float.Epsilon);
                Assert.AreEqual(0, Net.GetWeight(3, 2), float.Epsilon);
                Assert.AreEqual(0, Net.GetWeight(3, 3), float.Epsilon);
            }

            [TestMethod]
            public void GetSetState()
            {
                Assert.ThrowsException<ArgumentNullException>(
                    () => Net.SetState(null));
                Assert.ThrowsException<IndexOutOfRangeException>(
                    () => Net.SetState(new float[5]));
                Net.SetState(new float[] { 1, 2, 3, 4 });
                var state = Net.GetState();
                Assert.AreEqual(1, state[0]);
                Assert.AreEqual(2, state[1]);
                Assert.AreEqual(3, state[2]);
                Assert.AreEqual(4, state[3]);
            }

            [TestMethod]
            public void SetInputs()
            {
                Assert.ThrowsException<IndexOutOfRangeException>(
                    () => Net.SetInputs(new float[0]));
                Net.SetInputs(new float[] { 5 });
                var state = Net.GetState();
                Assert.AreEqual(5, state[0], float.Epsilon);

                Net.SetInput(0, 2);
                state = Net.GetState();
                Assert.AreEqual(2, state[0], float.Epsilon);
            }

            [TestMethod]
            public void GetOutputs()
            {
                Assert.ThrowsException<IndexOutOfRangeException>(
                    () => Net.GetOutput(1));
                Net.SetState(new float[] { 0, 5, 0, 0 });
                var output = Net.GetOutput(0);
                Assert.AreEqual(5, output);
                var allOuts = Net.GetOutputs();
                Assert.AreEqual(1, allOuts.Length);
                Assert.AreEqual(5, allOuts[0]);
            }

            [TestMethod]
            public void Reset()
            {
                Net.Activate(); Net.Activate();
                Assert.IsTrue(Net.GetState().Any(f => f != default));
                Net.Reset();
                Assert.IsTrue(Net.GetState().All(f => f == default));
            }

            [TestMethod]
            public void Activate()
            {
                var expected = new float[4];
                Net.SetInput(0, 1f);
                Net.Activate(); Net.Activate(); Net.Activate();
                var state = Net.GetState();
                expected[0] = 1;
                expected[2] = Net.ApplyTransferFunction(Net.GetWeight(0, 2) + Net.GetBias(2));
                expected[3] = Net.ApplyTransferFunction(Net.GetWeight(0, 3) + Net.GetBias(3));
                expected[1] = Net.ApplyTransferFunction(
                    (Net.GetWeight(2, 1) * expected[2]) + (Net.GetWeight(3, 1) * expected[3]) + Net.GetBias(1));
                Assert.AreEqual(expected[0], state[0], float.Epsilon);
                Assert.AreEqual(expected[1], state[1], float.Epsilon);
                Assert.AreEqual(expected[2], state[2], float.Epsilon);
                Assert.AreEqual(expected[3], state[3], float.Epsilon);
            }
        }
    }
}
