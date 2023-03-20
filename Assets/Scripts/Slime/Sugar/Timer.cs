using System;

namespace ROR.Lib
{
    public class Timer
    {
        public float Time { get; private set; }
        private float Inverval;
        public event Action<Timer, int> OnTick;

        public Timer(float time = 0f, float interval = 1f)
        {
            Reset(time, interval);
        }
        
        public bool Tick(float time)
        {
            var t1 = (int) (Time / Inverval);
            var t2 = (int) ((Time+time) / Inverval);

            Time += time;

            for (int i = 0; i < t2-t1; i++)
            {
                OnTick?.Invoke(this, t1 + 1 + i);
            }

            return t2-t1 > 0;
        }

        public void Reset(float time = 0f, float interval = 1f)
        {
            Time = time;
            Inverval = interval;
        }

        public void Clear()
        {
            Reset();
            OnTick = null;
        }
    }
}