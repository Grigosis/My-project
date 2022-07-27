using System.Collections.Generic;
using ROR.Core;

namespace RPGFight.Core
{
    

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