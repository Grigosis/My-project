using ROR.Core.Serialization;
using RPGFight.Core;

namespace ROR.Core
{
    public class PoisonEffectEntity : StackableEffectEntity
    {
        private float m_damagePerStack;
        public override void Init(IState istate)
        {
            base.Init(istate);
            m_damagePerStack = -((PoisonEffectDefinition) this.Definition).Damage;
        }

        public override void OnStacksChanged(int was, int now)
        {
            base.OnStacksChanged(was, now);
            Timer.Reset();
        }

        public override void OnTick(){
            base.OnTick();
            Target.Damage(new ElementDamage(0, m_damagePerStack * Stacks), this);
        }
    }
}