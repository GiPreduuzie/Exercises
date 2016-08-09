using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace QueueTests
{
    [TestClass]
    public class QueueTests
    {
        public QueueUsers<T> GetQueueUsers<T>()
        {
            return new QueueUsers<T>();
        }

        [TestMethod]
        public void TestMethod1()
        {
            GeneralCase();
        }

        private void SimpleCase()
        {
            var results = GetQueueUsers<int>()
               .Thread(1).Push(1)
               .Thread(1).Push(2)
               .Thread(1).Push(3)
               .Thread(2).Pop()
               .Thread(2).Pop()
               .Thread(2).Pop()
               .Run();

            CollectionAssert.AreEqual(new int[0], results.GetResultOfThread(1));
            CollectionAssert.AreEqual(new int[] { 1, 2, 3 }, results.GetResultOfThread(2));
            CollectionAssert.AreEqual(new int[0], results.GetQueueState());
        }

        private void GeneralCase()
        {
            var results = GetQueueUsers<int>()
               .Thread(2).Pop()
               .Thread(1).Push(1)
               .Thread(2).Pop()
               .Thread(1).Push(2)
               .Thread(1).Pop()
               .Thread(1).Pop()
               .Thread(2).Push(3)
               .Thread(2).Push(4)
               .Thread(2).Push(5)
               .Thread(1).Push(6)
               .Run();

            CollectionAssert.AreEqual(new [] { 3, 4}, results.GetResultOfThread(1));
            CollectionAssert.AreEqual(new [] { 1, 2 }, results.GetResultOfThread(2));
            CollectionAssert.AreEqual(new [] { 5, 6 }, results.GetQueueState());
        }
    }
}
