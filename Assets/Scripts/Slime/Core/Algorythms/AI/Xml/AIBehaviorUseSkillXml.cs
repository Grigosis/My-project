using System.Xml.Serialization;

namespace Assets.Scripts.Slime.Core.Algorythms
{
    public class AIBehaviorUseSkillXml : AIBehaviorActionXml
    {
        [XmlAttribute] 
        public string SkillType;
        
        [XmlAttribute] 
        public string TargetFilter;
        
        [XmlAttribute] 
        public string TargetSelectorFx;

        [XmlAttribute] 
        public int MaxMoveAPToCast = 0;
    }
}