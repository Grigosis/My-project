using System.Xml.Serialization;

namespace ROR.Core.Serialization
{
    public class Definition
    {
        [XmlAttribute]
        public string Id;
        
        public override string ToString()
        {
            return $"Definition [{Id}]";
        }
    }

    
}