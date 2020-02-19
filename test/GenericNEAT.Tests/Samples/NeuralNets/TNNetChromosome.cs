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
            [TestClass]
            public class Failure
            {
                [TestMethod]
                public void LayerSizes()
                {
                    Assert.Fail();
                }

                [TestMethod]
                public void SpecifyNumber()
                {
                    Assert.Fail();
                }
            }

            [TestClass]
            public class Success
            {
                [TestMethod]
                public void SuccessLayerSizes()
                {
                    var net = new NNetChromosome(1, 2, 2, 1);
                    Assert.AreEqual(1, net.InputCount);
                    Assert.AreEqual(2, net.OutputCount);

                    var ids = net.InputNeurons.Select(v => v.ID).ToArray();
                    Assert.AreEqual(1, ids.Length);
                    Assert.AreEqual(0u, ids[0]);
                    ids = net.OutputNeurons.Select(v => v.ID).ToArray();
                    Assert.AreEqual(2, ids.Length);
                    Assert.AreEqual(1u, ids[0]);
                    Assert.AreEqual(2u, ids[1]);
                    ids = net.HiddenNeurons.Select(v => v.ID).ToArray();
                    Assert.AreEqual(3, ids.Length);
                    Assert.AreEqual(3u, ids[0]);
                    Assert.AreEqual(4u, ids[1]);
                    Assert.AreEqual(5u, ids[2]);
                }

                [TestMethod]
                public void SpecifyNumber()
                {
                    Assert.Fail();
                }
            }
        }

        [TestMethod]
        public void Properties()
        {
            Assert.Fail();
        }
    }
}
