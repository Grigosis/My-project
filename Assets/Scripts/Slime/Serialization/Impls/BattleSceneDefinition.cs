﻿using System.Xml.Serialization;
using RPGFight;

namespace ROR.Core.Serialization
{
    public class LivingEntityDefinition : Definition
    {
        /// <summary>
        /// То что игрока по умолчанию. 
        /// </summary>
        public Attrs BaseAttributes = new Attrs();
        
        /// <summary>
        /// То что игрок сам вкачивал во время игры. 
        /// </summary>
        public Attrs UpgradedAttributes = new Attrs();
        
        [XmlArrayItem("Id")] 
        public string[] Skills;
        
        [XmlArrayItem("Id")] 
        public string[] EquippedItems;
    }

    /// <summary>
    /// Кукла персонажа, что куда может быть одето
    /// </summary>
    public class LivingEntityDoll
    {
        
    }

    public class EquipmentDefinition : Definition
    {
        public Attrs Attributes = new Attrs();
    }
    
    
    public class LivingStateInBattle
    {
        [XmlAttribute] public int x;
        [XmlAttribute] public int y;
        public Attrs Attributes;
    }

    public class MapState
    {

    }

    public class BattleSceneState
    {
        public MapState Map;

        [XmlArrayItem("Member")]
        public LivingStateInBattle[] Party1;

        [XmlArrayItem("Member")]
        public LivingStateInBattle[] Party2;
    }
}