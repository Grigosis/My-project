using ROR.Core;
using ROR.Core.Serialization;

namespace RPGFight.Core
{
    public class Builder
    {
        public static LivingEntity Build(LivingStateInBattle stateInBattle)
        {
            var livingEntity = new LivingEntity();
            livingEntity.InitAttrs(stateInBattle.Attributes);
            return livingEntity;
        }
    }
}