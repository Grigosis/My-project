using System;
using System.Collections.Generic;
using RPGFight.Library;
using UnityEngine;

namespace RPGFight.Core
{
    public class TimelineActions
    {
        public const int PLAYER_TURN = 1;
        public const int OTHER = 2;
    }
    
    public class TimelineEvent
    {
        public Action Action;
        public int Type;
        public object Object;
        public int Frame;

        public TimelineEvent(Action action, int type, object o)
        {
            Action = action;
            Type = type;
            Object = o;
        }
    }

    /// <summary>
    /// Class for high loaded actions that should be executed at exact time
    /// </summary>
    public class Timeline
    {
        public int CurrentFrame { get; protected set; } = 0;

        private List<DirtyList<TimelineEvent>> m_timeline;
        private int m_needSimulateAt = 0;
        private int m_totalActions = 0;

        public event Action<TimelineEvent> Added;
        public event Action<TimelineEvent> Removed;

        public Timeline(int count)
        {
            m_timeline = new List<DirtyList<TimelineEvent>>(count);
            for (int i = 0; i < count; i++)
            {
                m_timeline.Add(new DirtyList<TimelineEvent>());
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
        public TimelineEvent SimulateOneAction()
        {
            if (m_totalActions == 0)
            {
                Debug.Log("SimulateOneAction:m_totalActions == 0");
                return null;
            }

            TimelineEvent timelineEvent = null;
            while (true)
            {
                
                var dirtyList = m_timeline[CurrentFrame % m_timeline.Count];
                var needBreak = false;
                
                //Debug.Log($"SimulateOneAction:[{m_needSimulateAt}]/[{ dirtyList.Count}]");
                if (m_needSimulateAt < dirtyList.Count)
                {
                    timelineEvent = dirtyList[m_needSimulateAt];
                    timelineEvent.Action.Invoke();
                    m_needSimulateAt++;
                    m_totalActions--;
                    Removed?.Invoke(timelineEvent);
                    needBreak = true;
                }

                
                
                if (m_needSimulateAt >= dirtyList.Count)
                {
                    m_needSimulateAt = 0;
                    CurrentFrame++;
                    
                    dirtyList.Clear();
                    if (needBreak)
                    {
                        
                        return timelineEvent;
                    }
                }
            }

        }

        public void Add(int framesShift, TimelineEvent tlEvent)
        {
            if (framesShift < 0) throw new Exception();
            m_totalActions++;
            var at = CurrentFrame + framesShift;
            tlEvent.Frame = at;
            Added?.Invoke(tlEvent);
            Debug.Log("TIMELINE:ADDED: "+ CurrentFrame +"=>" +at + " " + tlEvent.Object);
            m_timeline[at % m_timeline.Count].Add(tlEvent);
        }

        public void Foreach(Action<TimelineEvent> action, int frames)
        {
            var need = m_needSimulateAt;
            for (int i = CurrentFrame; i < CurrentFrame+frames; i++)
            {
                var dirtyList = m_timeline[i % m_timeline.Count];
                for (int j = need; j < dirtyList.Count; j++)
                {
                    action(dirtyList[j]);
                }

                need = 0;
            }
        }
    }
}