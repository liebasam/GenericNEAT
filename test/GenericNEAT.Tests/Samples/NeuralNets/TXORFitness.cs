using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace GenericNEAT.Samples.NeuralNets.Tests
{
    [TestClass]
    public class TXORFitness
    {
        XORFitness fit = new XORFitness();

        [TestMethod]
        public void AllRight()
        {
            var net = new XORCorrect();
            Assert.AreEqual(4, fit.Evaluate(net), float.Epsilon);
        }

        [TestMethod]
        public void AllWrong()
        {
            var net = new XORWrong();
            Assert.AreEqual(0, fit.Evaluate(net), float.Epsilon);
        }

        [TestMethod]
        public void HalfRight()
        {
            var net = new XORHalfRight();
            Assert.AreEqual(2, fit.Evaluate(net), float.Epsilon);
        }
    }
}
