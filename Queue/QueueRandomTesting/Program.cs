using Queue;
using System;
using System.Threading;

namespace QueueRandomTesting
{
    class ToolSet<T>
    {
        public ThreadingQueue<T> Queue;
        public int RandomSeeed;
    }

    class Program
    {
        static void DoWork(object toolsetObject)
        {
            var toolset = toolsetObject as ToolSet<int>;
            var queue = toolset.Queue;
            var random = new Random(toolset.RandomSeeed);

            while (true)
            {
                var push = random.Next(2) % 2 == 0;

                if (push)
                {
                    var value = random.Next(100);
                    Console.WriteLine($"{Thread.CurrentThread.Name}: push {value}");
                    queue.Push(value);
                }
                else
                {
                    Console.WriteLine($"{Thread.CurrentThread.Name}: pop... ");
                    var value = queue.Pop();
                    Console.WriteLine($"{Thread.CurrentThread.Name}: ...{value} poped");
                }

                Thread.Sleep(random.Next(100, 500));
            }
        }

        static void DoPop(object toolsetObject)
        {
            var toolset = toolsetObject as ToolSet<int>;
            var queue = toolset.Queue;
            var random = new Random(toolset.RandomSeeed);

            while (true)
            {
                Console.WriteLine($"{Thread.CurrentThread.Name}: pop... ");
                var value = queue.Pop();
                Console.WriteLine($"{Thread.CurrentThread.Name}: ...{value} poped");

                Thread.Sleep(random.Next(100, 500));
            }
        }

        static void DoPush(object toolsetObject)
        {
            var toolset = toolsetObject as ToolSet<int>;
            var queue = toolset.Queue;
            var random = new Random(toolset.RandomSeeed);

            while (true)
            {
                var value = random.Next(100);
                Console.WriteLine($"{Thread.CurrentThread.Name}: push {value}");
                queue.Push(value);

                Thread.Sleep(TimeSpan.FromSeconds(5));
            }
        }

        static void Main(string[] args)
        {
            var queue = new ThreadingQueue<int>();

            for (int i = 0; i < 10; i++)
            {
                queue.Push(i);
            }

            var toolset1 = new ToolSet<int>() { Queue = queue, RandomSeeed = 1 };
            var toolset2 = new ToolSet<int>() { Queue = queue, RandomSeeed = 2 };
            var toolset3 = new ToolSet<int>() { Queue = queue, RandomSeeed = 3 };
            var toolset4 = new ToolSet<int>() { Queue = queue, RandomSeeed = 4 };

            var thread1 = new Thread(DoWork);
            thread1.Name = "A";
            var thread2 = new Thread(DoWork);
            thread2.Name = "B";
            var thread3 = new Thread(DoPush);
            thread3.Name = "C";
            var thread4 = new Thread(DoPop);
            thread4.Name = "D";

            thread1.Start(toolset1);
            thread2.Start(toolset2);
            thread3.Start(toolset3);
            thread4.Start(toolset4);


            Thread.Sleep(TimeSpan.FromMinutes(10));
        }
    }
}
