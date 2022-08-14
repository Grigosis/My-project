namespace Assets.Scripts.Slime.Core.Algorythms.Logic
{
    public class AIBehavior
    {
        public AIPositioning Positioning;

        public AIBehavior(AIBehaviorDefinition definition)
        {
            Positioning = new AIPositioning(definition.Positioning);
        }
    }
}