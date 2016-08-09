using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SumOfNumbers;
using System.Linq;

namespace SumOfNumbersTests
{
    [TestClass]
    public class SumOfNumbersTests
    {
        private IPartitionSearcher GetPartionSearcher()
        {
            return new PartitionSearcher();
        }

        private Tuple<int, int>[] EmptyResult
        {
            get { return new Tuple<int, int>[0]; }
        }

        [TestMethod]
        public void EmptyCollection()
        {
            var result = GetPartionSearcher().Search(new int[0], 8).ToArray();
            CollectionAssert.AreEquivalent(EmptyResult, result);
        }

        [TestMethod]
        public void GeneralCase()
        {
            var sum = 6;
            var input = new int[] { -1, 0, 4, 2, 7, 5, 13, 14 };

            var result = GetPartionSearcher().Search(input, sum).ToArray();

            CollectionAssert.AreEquivalent(new [] { Tuple.Create(7, -1), Tuple.Create(4, 2) }, result);
        }

        [TestMethod]
        public void MiddleCase()
        {
            var sum = 6;
            var input = new int[] { 3, 5, 3, 0};

            var result = GetPartionSearcher().Search(input, sum).ToArray();

            CollectionAssert.AreEquivalent(new [] { Tuple.Create(3, 3) }, result);
        }

        [TestMethod]
        public void AbsenceCase()
        {
            var sum = 6;
            var input = new int[] { 0, 4, -2, 5 };

            var result = GetPartionSearcher().Search(input, sum).ToArray();

            CollectionAssert.AreEquivalent(EmptyResult, result);
        }

        [TestMethod]
        public void SelfPairingIsNotPermitted()
        {
            var sum = 6;
            var input = new int[] { 3 };

            var result = GetPartionSearcher().Search(input, sum).ToArray();

            CollectionAssert.AreEquivalent(EmptyResult, result);
        }

        [TestMethod]
        public void LeftPitfall()
        {
            var sum = 6;
            var input = new int[] { -1000, 3, 3 };

            var result = GetPartionSearcher().Search(input, sum).ToArray();

            CollectionAssert.AreEquivalent(new[] { Tuple.Create(3, 3) }, result);
        }
    }
}
