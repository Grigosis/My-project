using System.Xml.Serialization;

namespace Assets.Scripts.Slime.Core.Algorythms
{
    public class AIBehaviorTriggerUnitDiedXml : AIBehaviorTriggerXml
    {
        [XmlAttribute] 
        public bool Ally;
    }
}