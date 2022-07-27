using System.Globalization;
using ROR.Core.Serialization;
using RPGFight;
using UnityEngine;

namespace ROR.Core
{
    public class HealingEffectEntity : StackableEffectEntity
    {
        private float m_damagePerStack;
        public override void Init(IState istate)
        {
            base.Init(istate);
            m_damagePerStack = ((HealingEffectDefinition) this.Definition).Heal;
        }

        public override void OnStacksChanged(int was, int now)
        {
            base.OnStacksChanged(was, now);
            Timer.Reset();
        }

        public override void OnTick(){
            base.OnTick();

            var heal = m_damagePerStack * Stacks;
            Target.Heal(heal, this);

            DI.CreateFloatingText(Target, heal.ToString(CultureInfo.InvariantCulture), Color.green);
        }
    }
}