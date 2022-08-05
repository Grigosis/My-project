using System.Collections.Generic;
using System.Xml.Serialization;
using ROR.Core.Serialization;
using ROR.Lib;
using UnityEngine.Assertions;

namespace ROR.Core
{
    public class EffectState : IState
    {
        public string EffectDefinition;
        public Dictionary<string, object> Data;
        public long CasterId;
        public long TargetId;
        
        [XmlIgnore]
        public LivingEntity Caster
        {
            get => Entities.Get<LivingEntity>(CasterId);
            set
            {
                Assert.IsTrue(value.EntityId != 0);
                CasterId = value.EntityId;
            }
        }

        [XmlIgnore]
        public LivingEntity Target
        {
            get => Entities.Get<LivingEntity>(TargetId);
            set
            {
                Assert.IsTrue(value.EntityId != 0);
                TargetId = value.EntityId;
            }
        }
    }

    [System.Serializable]
    public abstract class EffectEntity : ICreatable
    {
        public LivingEntity Caster { get; private set; }
        public LivingEntity Target { get; private set; }

        protected IntervalTimer Timer = new IntervalTimer();
        public EffectDefinition Definition {get; private set; }
        public float Time => Timer.Time;
        public float MaxTime => Definition.Duration;
        public bool IsAttached { get; private set; }
        public Dictionary<string, object> Data;

        private int m_stacks = 1;
        public int Stacks {
            get
            {
                return m_stacks;
            }
            set
            {
                if (m_stacks != value)
                {
                    var was = m_stacks;
                    m_stacks = value;
                    OnStacksChanged(was, value);
                }
            }
        }


        public virtual void OnStacksChanged(int was, int now) { }
        
        public virtual void Init(IState istate)
        {
            var state = (EffectState) istate;
            Definition = D.Instance.Get<EffectDefinition>(state.EffectDefinition);
            Caster = state.Caster; 
            Target = state.Target;

            Assert.IsTrue(Target != null, "TARGET IS NULL");
            Assert.IsTrue(Caster != null, "CASTER IS NULL");
            
            Data = state.Data;
            Timer.Reset(Definition.TickInterval, Definition.TickInterval);
        }

        public IState GetState()
        {
            return new EffectState()
            {
                Caster = this.Caster,
                Target = this.Target,
                Data = this.Data,
                EffectDefinition = this.Definition.Id,
            };
        }

        public void Attach()
        {
            Target.EffectBar.AddEffect(this);
        }
        
        public void Detach()
        {
            Target.EffectBar.RemoveEffect(this);
        }

        internal virtual void OnAdded()
        {
            IsAttached = true;
        }

        internal virtual void OnRemoved()
        {
            IsAttached = false;
        }
        
        public virtual bool Join(EffectBar bar)
        {
            return false;
        }

        public virtual void OnTick() { }

        public void Update(float delta)
        {
            if (!IsAttached) return;
            
            if (Timer.Tick(delta))
            {
                OnTick();
            }

            if (Timer.Time >= MaxTime)
            {
                Detach();
            }
        }
    }
}
