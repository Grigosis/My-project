using System;
using System.Collections.Generic;
using Assets.Scripts.Slime.Core.BattleMap.Logic.Interfaces;
using ROR.Core;
using RPGFight.Core;

namespace Assets.Scripts.Slime.Core.Skills
{
    public interface ISkillImplementation
    {
        void PredictFuture(SkillEntity skillEntity, List<ISkillTarget> targets, Random seed);
        void ExecuteFuture(Future future, bool animate);
        void CastSkill(SkillEntity skillEntity, List<ISkillTarget> targets, Random seed);
    }

    public class Future
    {
        public static Random Random = new Random();
        //public List<Future> Nodes = new List<Future>();
        private event Action<Future> OnExecuted;
        public virtual void Execute()
        {
            TriggerOnExecuted();
        }

        protected void TriggerOnExecuted()
        {
            OnExecuted?.Invoke(this);
        }
    }
    
    public class Brancher : Future
    {
        public double HitChance;
        public bool Hitted;
        public Future Success;
        public Future Failure;

        public override void Execute()
        {
            base.Execute();
            if (HitChance > Random.NextDouble())
            {
                Success?.Execute();
            }
            else
            {
                Failure.Execute();
            }
        }
    }

    public class DamageFuture : Future {
        public double CritChance;
        public List<ElementDamage> Damage;
        public LivingEntity Entity;
        public EffectEntity Effect;
    }

    public class HitCondition : Brancher
    {
        public double HitChance;
        public bool Hitted;
        public override void Execute()
        {
            base.Execute();
        }
    }
}