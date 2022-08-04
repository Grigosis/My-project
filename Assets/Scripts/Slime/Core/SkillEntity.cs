using Assets.Scripts.Slime.Core.Skills;
using ROR.Core;
using ROR.Core.Serialization;

namespace Assets.Scripts.Slime.Core
{
    public class SkillEntity
    {
        public int Cooldown = 5;
        public SkillDefinition Definition;
        public LivingEntity Owner;
        public ISkillImplementation Implementation;
        public SkillEntity(LivingEntity owner, SkillDefinition definition)
        {
            Definition = definition;
            Implementation = R.CreateInstance<ISkillImplementation>(Definition.Implementation);
            Owner = owner;
        }

        public int GetMinRange()
        {
            return Definition.MinRange;
        }
        
        public int GetMaxRange()
        {
            return Definition.MaxRange;
        }
    }
}