using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

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
            Debug.LogWarning($"Measure:{name} totalMs:{timer.Elapsed.TotalMilliseconds}");
        }
    }
}