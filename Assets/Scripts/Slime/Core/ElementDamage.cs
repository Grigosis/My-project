using System.Xml.Serialization;

namespace RPGFight.Core
{
    public struct ElementDamage
    {
        [XmlAttribute] public int Id;
        [XmlAttribute] public double Min;
        [XmlAttribute] public double Max;

        public ElementDamage(int id, double min, double max)
        {
            Id = id;
            Min = min;
            Max = max;
        }

        public override string ToString()
        {
            return $"{Attrs.GetElementName(Id)}/{Min}-{Max}";
        }
    }
}