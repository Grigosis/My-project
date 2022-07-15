using System.Xml.Serialization;

namespace RPGFight.Core
{
    public struct ElementDamage
    {
        [XmlAttribute] public int Id;
        [XmlAttribute] public double Damage;

        public ElementDamage(int id, double damage)
        {
            Id = id;
            Damage = damage;
        }

        public override string ToString()
        {
            return $"{Attrs.GetElementName(Id)}/{Damage}";
        }
    }
}