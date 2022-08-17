using System;

namespace ClassLibrary1
{
    public class Scheduler
    {
        public static void Schedule(Action action)
        {
            action.Invoke();
        }
    }
}