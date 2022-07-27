using System;
using System.Diagnostics;

namespace Assets.Scripts.Sugar
{
    class Measure : IDisposable
    {
        private Stopwatch timer;
        private string name;
        public Measure(string name = "")
        {
            timer = new Stopwatch();
            timer.Start();
            this.name = name;
        }
        public void Dispose()
        {
            Console.WriteLine("Measure:"+name+" totalMs:" +  timer.Elapsed.TotalMilliseconds);
        }
    }
}