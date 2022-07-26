using ROR.Core.Serialization;

namespace Assets.Scripts.Slime.Core
{
    
    public class SkillEntity
    {
        public int Cooldown = 5;
        public SkillDefinition Definition;

        public SkillEntity(SkillDefinition definition)
        {
            Definition = definition;
        }

        public int GetRange()
        {
            return 10;
        }
    }
}