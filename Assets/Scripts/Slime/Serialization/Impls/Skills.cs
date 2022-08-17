using System.Xml.Serialization;
using Assets.Scripts.Slime.Core.Algorythms.Data;

namespace ROR.Core.Serialization
{
    public class ElementAttack
    {
        [XmlAttribute] public string Name;
        [XmlAttribute] public float Percent;

        public ElementAttack()
        {
        }

        public ElementAttack(string name, float percent)
        {
            Name = name;
            Percent = percent;
        }
    }

    public class SkillDefinition : BaseDefinition
    {
        [XmlArrayItem("Attack")]
        public ElementAttack[] Attacks;

        [XmlAttribute]
        public string TargetSelector;
        
        [XmlAttribute]
        public string UnityTargetSelector;
        
        [XmlAttribute]
        public string SplashProvider;
        
        [XmlAttribute]
        public string TargetRanger;
        
        [XmlAttribute]
        public string Implementation;

        [XmlAttribute]
        public RelationShip RelationShipFilter = RelationShip.Any;

        [XmlAttribute]
        public bool IsRangedAttack;

        [XmlAttribute]
        public float HitChanceMlt = 1;

        [XmlAttribute]
        public int SplashRange = 0;
        
        [XmlAttribute]
        public int AP = 0;
        
        [XmlAttribute]
        public int MinRange = 0;
        
        [XmlAttribute]
        public int MaxRange = 1;
        
        [XmlAttribute]
        public int Cooldown = 10;
    }
}