using System;
using System.Diagnostics;

namespace ContactBook.Controllers
{
    public class StopWatchCalculator : IDisposable
    {
        private readonly Stopwatch stopwatch = new Stopwatch();

        private readonly Action<TimeSpan> stopAction; 

        public StopWatchCalculator(Action<TimeSpan> stopAction)
        {
            this.stopAction = stopAction;

            stopwatch.Start();
        }

        public void Dispose()
        {
            stopwatch.Stop();

            stopAction(stopwatch.Elapsed);
        }
    }
}