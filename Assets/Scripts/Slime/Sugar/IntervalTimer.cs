using System;

namespace ROR.Lib
{
    
    public class IntervalTimer
    {
        public float Time { get; private set; }
        private float Interval;
        private bool TickOnZeroTime = false;
        public event Action<IntervalTimer, int> OnTick;

        public IntervalTimer(float time = 0f, float interval = 1f, bool tickOnZeroTime = false)
        {
            TickOnZeroTime = tickOnZeroTime;
            Reset(time, interval);
        }

        private int getTick(float t)
        {
            if (TickOnZeroTime && t == 0)
            {
                return -1;
            }
            return (int) (t / Interval);
        }
        
        public bool Tick(float time)
        {
            var t1 = getTick(Time);
            var t2 = getTick(Time+time);

            Time += time;

            for (int i = 0; i < t2-t1; i++)
            {
                OnTick?.Invoke(this, t1 + 1 + i);
            }

            return t2-t1 > 0;
        }

        public void Reset(float time, float interval)
        {
            Time = time;
            Interval = interval;
        }
        
        public void Reset(float time = 0f)
        {
            Time = time;
        }

        public void Clear()
        {
            Reset();
            OnTick = null;
        }
    }
}