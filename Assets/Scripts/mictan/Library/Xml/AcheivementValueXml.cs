using System.Xml.Serialization;

namespace ClassLibrary1.Xml
{

    public class AcheivementDoubleValueXml : AcheivementValueXml
    {
        [XmlAttributeAttribute()]
        public double Value;
    }
    
    public class AcheivementValueXml
    {
        [XmlAttributeAttribute()]
        public string Id;
    }
}