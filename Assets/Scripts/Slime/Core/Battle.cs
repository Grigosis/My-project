using System.Collections.Generic;
using ROR.Core;

namespace RPGFight.Core
{
    public class Battle
    {
        public readonly Timeline Timeline = new Timeline(700);
        public readonly List<Team> Teams = new List<Team>();


        public void Simulate()
        {
            Timeline.SimulateOneAction();
        }


    }

    public class Team
    {
        private List<LivingEntity> LivingEntities = new List<LivingEntity>();

        public void Add(LivingEntity le)
        {
            LivingEntities.Add(le);
            le.OnDeath += OnDeath;
        }

        private void Remove(LivingEntity obj)
        {
            LivingEntities.Remove(obj);
        }

        private void OnDeath(LivingEntity obj)
        {
            Remove(obj);
        }

    }
}