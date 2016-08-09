using System.Collections.Generic;
using Queue;
using System.Linq;

namespace QueueTests
{
    public class QueueUsingResults<T>
    {
        private ThreadingQueue<T> _queue;
        private Dictionary<int, T[]> _dictionary;

        public QueueUsingResults(ThreadingQueue<T> queue, Dictionary<int, T[]> dictionary)
        {
            _queue = queue;
            _dictionary = dictionary;
        }

        public T[] GetResultOfThread(int id)
        {
            return _dictionary[id];
        }

        public T[] GetQueueState()
        {
            return _queue.ToArray();
        }
    }
}