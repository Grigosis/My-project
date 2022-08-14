using System.Xml.Serialization;

namespace Assets.Scripts.Slime.Core.Algorythms
{
    public class FxParamXml
    {
        [XmlAttribute] 
        public string Name;
        
        [XmlAttribute] 
        public string Value;
    } 
}