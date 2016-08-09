using Queue;
using System;
using System.Diagnostics;
using System.Threading;

namespace QueueTests
{
    public class OperationSelector<T>
    {
        private readonly QueueUsers<T> _back;
        private readonly Action<Func<ThreadingQueue<T>, Result<T>>> _toPlan;

        public OperationSelector(QueueUsers<T> queueUsers, Action<Func<ThreadingQueue<T>, Result<T>>> toPlan)
        {
            _back = queueUsers;
            _toPlan = toPlan;
        }

        public QueueUsers<T> Push(T value)
        {
            _toPlan(x =>
            {
                Debug.WriteLine($"thread {Thread.CurrentThread.Name} pushes {value}");
                x.Push(value);

                return new Result<T>(default(T), false);
            });
            return _back;
        }

        public QueueUsers<T> Pop()
        {
            _toPlan(x =>
            {
                Debug.WriteLine($"thread '{Thread.CurrentThread.Name}' pops... ");
                var value = x.Pop();
                Debug.WriteLine($"... '{Thread.CurrentThread.Name}' poped {value}");

                return new Result<T>(value, true);
            });
            return _back;
        }
    }
}
