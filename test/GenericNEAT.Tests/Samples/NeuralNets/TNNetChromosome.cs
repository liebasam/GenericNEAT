using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericNEAT.Samples.NeuralNets.Tests
{
    [TestClass]
    public class TNNetChromosome
    {
        [TestClass]
        public class Constructors
        {
            [TestMethod]
            public void Failure()
            {
                Assert.ThrowsException<ArgumentOutOfRangeException>(
                    () => new NNetChromosome(0, 1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(
                    () => new NNetChromosome(1, 0));
                Assert.ThrowsException<ArgumentOutOfRangeException>(
                    () => new NNetChromosome(1, 1, -1));
            }

            [TestMethod]
            public void Success()
            {
                var net = new NNetChromosome(1, 2, 1);
                Assert.AreEqual(4, net.VertexCount);
                Assert.AreEqual(1, net.InputCount);
                Assert.AreEqual(2, net.OutputCount);
                Assert.AreEqual(1, net.HiddenCount);
            }
        }

        [TestClass]
        public class Fields
        {
            static NNetChromosome net;

            [ClassInitialize]
            public static void ClassInit(TestContext tc)
            {
                net = new NNetChromosome(1, 2, 1);
            }

            [TestMethod]
            public void EnumerateInputs()
            {
                var ins = net.InputNeurons.ToArray();
                Assert.AreEqual(1, ins.Length);
                Assert.AreEqual(0u, ins[0].ID);
            }

            [TestMethod]
            public void EnumerateOutputs()
            {
                var outs = net.OutputNeurons.ToArray();
                Assert.AreEqual(2, outs.Length);
                Assert.AreEqual(1u, outs[0].ID);
                Assert.AreEqual(2u, outs[1].ID);
            }

            [TestMethod]
            public void EnumerateHidden()
            {
                var hidden = net.HiddenNeurons.ToArray();
                Assert.AreEqual(1, hidden.Length);
                Assert.AreEqual(3u, hidden[0].ID);
            }
        }

        [TestClass]
        public class Methods
        {
            static NNetChromosome net;

            [ClassInitialize]
            public static void ClassInit(TestContext tc)
            {
                net = new NNetChromosome(1, 2, 1);
            }

            [TestMethod]
            public void Clone()
            {
                var other = net.Clone() as NNetChromosome;
                Assert.IsNotNull(other);
                Assert.AreNotSame(net, other);
                Assert.AreEqual(net.VertexCount, other.VertexCount);
                Assert.AreEqual(net.InputCount, other.InputCount);
                Assert.AreEqual(net.OutputCount, other.OutputCount);
                Assert.AreEqual(net.HiddenCount, other.HiddenCount);
                Assert.AreEqual(net[0].ToString(), other[0].ToString());
                Assert.AreEqual(net[1].ToString(), other[1].ToString());
                Assert.AreEqual(net[2].ToString(), other[2].ToString());
                Assert.AreEqual(net[3].ToString(), other[3].ToString());
            }

            [TestMethod]
            public void CreateNew()
            {
                var other = net.CreateNew() as NNetChromosome;
                Assert.IsNotNull(other);
                Assert.AreNotSame(net, other);
                Assert.AreEqual(net.VertexCount, other.VertexCount);
                Assert.AreEqual(net.InputCount, other.InputCount);
                Assert.AreEqual(net.OutputCount, other.OutputCount);
                Assert.AreEqual(net.HiddenCount, other.HiddenCount);
                Assert.AreNotEqual(net[0].ToString(), other[0].ToString());
                Assert.AreNotEqual(net[1].ToString(), other[1].ToString());
                Assert.AreNotEqual(net[2].ToString(), other[2].ToString());
                Assert.AreNotEqual(net[3].ToString(), other[3].ToString());
            }
        }
    }
}
