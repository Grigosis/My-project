using ROR.Core.Serialization;
using UnityEngine;

namespace Assets.Scripts.Slime.Core.Algorythms
{
    
    
    [CreateAssetMenu(fileName = "AIBehavior", menuName = "GameItems/Other/AI", order = 51)]
    public class AIBehaviorDefinition : Definition
    {
        public AIPositioningXml Positioning;
        
        public AIBehaviorActionXml[] Behavior;
        
        public AIBehaviorTriggerXml[] Triggers;
    }
}