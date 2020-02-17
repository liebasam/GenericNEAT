using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericNEAT.Operators.Tests
{
    [TestClass]
    public class TIDFactory
    {
        [TestClass]
        public class NextID
        {
            [TestMethod]
            public void IDNotInCache()
            {
                var factory = new IDFactory();
                Assert.AreEqual(0u, factory.GetID(0, 0));
                Assert.AreEqual(1u, factory.GetID(0, 1));
                Assert.AreEqual(2u, factory.GetID(1, 0));
            }

            [TestMethod]
            public void IDInCache()
            {
                var factory = new IDFactory();
                factory.GetID(0, 0);
                Assert.AreEqual(0u, factory.GetID(0, 0));
            }

            [TestMethod]
            public void ClearIDCacheAndIncrementNextID()
            {
                var factory = new IDFactory();
                factory.GetID(0, 0);
                factory.GetID(0, 1);
                factory.ClearCacheAndIncrementNextID();
                Assert.AreEqual(2u, factory.GetID(0, 0));
                Assert.AreEqual(3u, factory.GetID(0, 1));
            }
        }
    }
}
