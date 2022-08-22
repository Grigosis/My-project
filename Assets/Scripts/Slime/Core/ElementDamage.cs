using System.Xml.Serialization;

namespace RPGFight.Core
{
    public struct ElementDamage
    {
        [XmlAttribute] public int Id;
        [XmlAttribute] public double Min;
        [XmlAttribute] public double Max;
        [XmlAttribute] public double CritMin;
        [XmlAttribute] public double CritMax;
        [XmlAttribute] public bool IsCrit;

        public ElementDamage(int id, double min, double max, double critMin, double critMax)
        {
            Id = id;
            Min = min;
            Max = max;
            CritMin = critMin;
            CritMax = critMax;
            IsCrit = false;
        }

        public override string ToString()
        {
            return $"{Attrs.GetElementName(Id)}/{Min}-{Max}";
        }
    }
}