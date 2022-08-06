﻿using System.Xml.Serialization;

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
        public string Implementation;
        
        [XmlAttribute]
        public bool IsRangedAttack;

        [XmlAttribute]
        public float HitChanceMlt = 1;

        [XmlAttribute]
        public int MinRange = 0;
        
        [XmlAttribute]
        public int MaxRange = 1;
        
        [XmlAttribute]
        public int Cooldown = 10;
    }
}