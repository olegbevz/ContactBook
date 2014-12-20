using System;
using System.Diagnostics;

namespace ContactBook.Performance
{
    public class StopWatchCalculator : IDisposable
    {
        private readonly Stopwatch stopwatch = new Stopwatch();

        public StopWatchCalculator()
        {
            stopwatch.Start();
        }

        public TimeSpan ElapsedTime
        {
            get { return stopwatch.Elapsed; }
        }

        public void Dispose()
        {
            stopwatch.Stop();
        }
    }
}