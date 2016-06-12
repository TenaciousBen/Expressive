using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Expressive.Tests.Helpers
{
    public class TimeAssertion : IDisposable
    {
        public int Milliseconds { get; set; }
        public Stopwatch Stopwatch { get; set; }

        public TimeAssertion(int milliseconds)
        {
            Milliseconds = milliseconds;
            Stopwatch = new Stopwatch();
            Stopwatch.Start();
        }

        public void Dispose()
        {
            Stopwatch.Stop();
            Assert.IsTrue(Stopwatch.ElapsedMilliseconds <= Milliseconds);
        }
    }
}
