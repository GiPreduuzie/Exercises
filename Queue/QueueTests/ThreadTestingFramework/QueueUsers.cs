using Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace QueueTests
{
    public class QueueUsers<T>
    {
        private class ThreadToolset<T>
        {
            public ThreadToolset(ThreadingQueue<T> queue, ThreadDescription<T> threadDescription)
            {
                Queue = queue;
                ThreadDescription = threadDescription;
            }

            public ThreadingQueue<T> Queue { get; private set; }
            public ThreadDescription<T> ThreadDescription { get; private set; }
        }

        List<Tuple<int, Func<ThreadingQueue<T>, Result<T>>>> _plan
            = new List<Tuple<int, Func<ThreadingQueue<T>, Result<T>>>>();

        public OperationSelector<T> Thread(int id)
        {
            return new OperationSelector<T>(this, x => _plan.Add(Tuple.Create(id, x)));
        }

        public QueueUsingResults<T> Run()
        {
            var threads = new Dictionary<int, ThreadDescription<T>>();

            var time = new Time();
            foreach (var record in _plan)
            {
                var threadKey = record.Item1;
                var threadAction = record.Item2;

                var description = threads.GetWithDefault(threadKey, x => new ThreadDescription<T>(x));
                description.Add(threadAction, time);
                threads[threadKey] = description;

                time.Forward();
            }

            var threadObjects = new List<Thread>();
            var queue = new ThreadingQueue<T>();
            foreach (var threadDescription in threads)
            {
                var thread = new Thread(RunThread);
                thread.Name = threadDescription.Key.ToString();
                thread.Start(new ThreadToolset<T>(queue, threadDescription.Value));
                threadObjects.Add(thread);
            }

            foreach (var thread in threadObjects)
            {
                thread.Join();
            }

            return new QueueUsingResults<T>(
                queue,
                threads.ToDictionary(x => x.Key, x => x.Value.GetResults().ToArray()));
        }

        public static void RunThread(object toolsetObject)
        {
            var toolset = toolsetObject as ThreadToolset<T>;

            foreach (var action in toolset.ThreadDescription.GetBehavior())
            {
                toolset.ThreadDescription.AddResult(action(toolset.Queue));
            }
        }
    }
}
