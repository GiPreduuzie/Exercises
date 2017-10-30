using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleArbitraryPrecisionArithmetic.Tests
{
    [TestClass]
    public class MyLongTests
    {
        [TestMethod]
        public void Construction()
        {
            Assert.AreEqual("123", MyLong.FromInt(123).ToString());
            Assert.AreEqual("0", MyLong.FromInt(0).ToString());

            Assert.AreEqual("123", MyLong.FromString("123").ToString());
            Assert.AreEqual("0", MyLong.FromString("0").ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void WrongConstruction()
        {
            MyLong.FromInt(-123);
        }

        [TestMethod]
        public void Sum()
        {
            Assert.AreEqual("3", (MyLong.FromInt(1) + MyLong.FromInt(2)).ToString());
            Assert.AreEqual("123", (MyLong.FromInt(0) + MyLong.FromInt(123)).ToString());
            Assert.AreEqual("100", (MyLong.FromInt(99) + MyLong.FromInt(1)).ToString());
        }
    }
}
