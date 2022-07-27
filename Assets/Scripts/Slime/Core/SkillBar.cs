using System.Collections.Generic;

namespace Assets.Scripts.Slime.Core
{
    public class SkillBar
    {
        public List<SkillEntity> SkillEntities = new List<SkillEntity>();

        public void Add(SkillEntity skillEntity)
        {
            SkillEntities.Add(skillEntity);
        }
        
        public void Update()
        {
            for (var i = 0; i < SkillEntities.Count; i++)
            {
                var se = SkillEntities[i];
                se.Cooldown--;
            }
        }
    }
}