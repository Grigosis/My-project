using System.Xml.Serialization;
using ROR.Core.Serialization;

namespace ClassLibrary1.Inventory
{
    public class ItemDefinition : BaseDefinition
    {
        [XmlAttribute]
        public int ItemTypes;
        
        [XmlAttribute]
        public int StackSize;
        
        [XmlAttribute]
        public float Weight;
        
        [XmlAttribute]
        public bool Sellable;
        
        [XmlAttribute]
        public bool Storable;
        
        [XmlAttribute]
        public bool Equipable;
        
        [XmlAttribute]
        public int GearFlags;
    }
    
    public class GearType
    {
        public const int Helmet = 1;
        public const int Body = 2;
        public const int Gloves = 4;
        public const int Boots = 8;
        public const int Amulet = 16;
        
        public const int Ring = 32;
        public const int MainHand = 64;
        public const int OffHand = 128;
        public const int LargeWeapon = 256;
    }
}