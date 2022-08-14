using System.Xml.Serialization;
using ROR.Core.Serialization;

namespace Assets.Scripts.Slime.Core.Algorythms
{
    public class AIBehaviorDefinition : Definition
    {
        public AIPositioningXml Positioning;
        
        [XmlArrayItem("Action")]
        public AIBehaviorActionXml[] Behavior;
        
        [XmlArrayItem("Trigger")]
        public AIBehaviorTriggerXml[] Triggers;
    }
}