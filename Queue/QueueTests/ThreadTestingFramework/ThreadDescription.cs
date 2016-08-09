using Queue;
using System;
using System.Collections.Generic;
using System.Threading;

namespace QueueTests
{
    internal class ThreadDescription<T>
    {
        private int _threadId;
        private int _lastBenchmark = Time.StartTime;
        private List<Func<ThreadingQueue<T>, Result<T>>> _threadBehavior
            = new List<Func<ThreadingQueue<T>, Result<T>>>();
        private List<T> _results = new List<T>();

        public ThreadDescription(int threadId)
        {
            _threadId = threadId;
        }

        internal void Add(Func<ThreadingQueue<T>, Result<T>> action, Time time)
        {
            var sleepPeriod = time.GetSleepPeriod(_lastBenchmark);
            _threadBehavior.Add(x =>
            {
                Thread.Sleep(sleepPeriod);
                return new Result<T>(default(T), false);
            });

            _threadBehavior.Add(action);

            _lastBenchmark = time.GetBenchmark();
        }

        internal IEnumerable<Func<ThreadingQueue<T>, Result<T>>> GetBehavior()
        {
            return _threadBehavior;
        }

        internal void AddResult(Result<T> result)
        {
            if (result.Exists)
                _results.Add(result.Value);
        }

        public IEnumerable<T> GetResults() { return _results; }
    }
}