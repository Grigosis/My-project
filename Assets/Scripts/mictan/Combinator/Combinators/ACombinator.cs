using System;

namespace Combinator
{
    public abstract class ACombinator<OBJ, OUT> {
        
        public OUT Value { get; protected set; }
        public event Action<ICombinator> OnChanged;
        
        protected OBJ obj;
        protected bool m_IsDirty = true;
        protected bool m_liveUpdates = false;
        
        public string NodeDebugName { get; set; }
        public bool IsDependent { get; set; }

        public void SetObject(object obj)
        {
            if (obj is OBJ ob)
            {
                SetObject(ob);
            }
            else
            {
                throw new Exception($"Wrong type of obj. {obj} is not type of {typeof(OBJ)}");
            }
            
        }
        public void SetObject(OBJ obj)
        {
            this.obj = obj;
            MarkForRecalculate();
        }

        public virtual void SetLiveUpdates(bool liveUpdates, bool recalculate = true)
        {
            if (liveUpdates == m_liveUpdates)
            {
                return;
            }

            if (!liveUpdates)
            {
                m_liveUpdates = false;
                return;
            }

            m_liveUpdates = true;
            if (m_IsDirty && recalculate)
            {
                MarkForRecalculate();
            }
        }

        public abstract OUT Calculate();

        public virtual void MarkForRecalculate()
        {
            if (!m_liveUpdates)
            {
                m_IsDirty = true;
                return;
            }
            
            var value = Calculate();
            m_IsDirty = false;
            if (!value.Equals(Value))
            {
                Value = value;
                OnChanged?.Invoke((ICombinator)this);
            }
        }
    }
}