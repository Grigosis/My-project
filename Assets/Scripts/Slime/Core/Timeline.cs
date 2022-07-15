using System;
using System.Collections.Generic;
using RPGFight.Library;

namespace RPGFight.Core
{
    public struct TimelineAction
    {
        public Action Action;
        public int Type;
        public object Object;
    }

    /// <summary>
    /// Class for high loaded actions that should be executed at exact time
    /// </summary>
    public class Timeline
    {
        public int CurrentFrame { get; protected set; } = 0;

        private List<DirtyList<TimelineAction>> m_timeline;
        private int m_needSimulateAt = 0;
        private int m_totalActions = 0;

        public Timeline(int count)
        {
            m_timeline = new List<DirtyList<TimelineAction>>(count);
            for (int i = 0; i < count; i++)
            {
                m_timeline.Add(new DirtyList<TimelineAction>());
            }
        }


        /// <summary>
        /// Simulates one frame, executing all actions
        /// </summary>
        public void SimulateWholeFrame()
        {
            var dirtyList = m_timeline[CurrentFrame % m_timeline.Count];

            for (int i = 0; i < dirtyList.Count; i++)
            {
                dirtyList[i].Action.Invoke();
            }
            m_totalActions-=dirtyList.Count;
            dirtyList.Clear();
            CurrentFrame++;
        }



        /// <summary>
        /// Simulates one action no matter on which frame it is
        /// </summary>
        public void SimulateOneAction()
        {
            if (m_totalActions == 0)
            {
                return;
            }

            while (true)
            {
                var dirtyList = m_timeline[CurrentFrame % m_timeline.Count];
                var needBreak = false;
                if (m_needSimulateAt < dirtyList.Count)
                {
                    dirtyList[m_needSimulateAt].Action.Invoke();
                    m_needSimulateAt++;
                    m_totalActions--;
                    needBreak = true;
                }

                if (m_needSimulateAt >= dirtyList.Count)
                {
                    m_needSimulateAt = 0;
                    CurrentFrame++;
                    dirtyList.Clear();
                    if (needBreak)
                    {
                        break;
                    }
                }
            }

        }

        public void Add(int framesShift, TimelineAction action)
        {
            if (framesShift < 0) throw new Exception();
            m_totalActions++;
            var at = CurrentFrame + framesShift;
            m_timeline[at % m_timeline.Count].Add(action);
        }
    }
}