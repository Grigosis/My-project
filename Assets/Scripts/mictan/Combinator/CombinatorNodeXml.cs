using System.Xml.Serialization;

namespace Combinator
{
    
    public class CombinatorXml
    {
        public CombinatorNodeXml Root;
    }
    
    public class CombinatorNodeXml
    {
        [XmlAttribute]
        public string Value;
        
        [XmlAttribute]
        public string Fx;
        
        [XmlArrayItem("Node")]
        public CombinatorNodeXml[] Nodes = new CombinatorNodeXml[0];
    }
}