using System;

namespace QueueTests
{
    public class Time
    {
        private int _steps = 0;

        public Time()
        {
        }

        public static int StartTime { get { return 0; } }

        public void Forward()
        {
            _steps++;
        }

        public int GetBenchmark()
        {
            return _steps;
        }

        public TimeSpan GetSleepPeriod(int lastBenchmark)
        {
            return TimeSpan.FromMilliseconds((_steps - lastBenchmark) * 100);
        }
    }
}