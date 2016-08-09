using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Queue
{
    public class ThreadingQueue<T> : IEnumerable<T>
    {
        private readonly Queue<T> _queue = new Queue<T>();
        private readonly AutoResetEvent autoEvent = new AutoResetEvent(false);

        public T Pop()
        {
            lock (_queue)
            {
                if (_queue.Count > 1)
                {
                    return _queue.Dequeue();
                }
            }

            autoEvent.WaitOne();

            lock (_queue)
            {
                return _queue.Dequeue();
            }
        }

        public void Push(T value)
        {
            lock (_queue)
            {
                _queue.Enqueue(value);

                if (_queue.Count == 1)
                {
                    autoEvent.Set();
                }
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _queue.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _queue.GetEnumerator();
        }
    }
}
