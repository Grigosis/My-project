using System.Xml.Serialization;

namespace Assets.Scripts.Slime.Core.Algorythms
{
    public class AIPositioningLayerXml
    {
        [XmlAttribute] 
        public float MinValue;
        
        [XmlAttribute] 
        public float MaxValue;

        [XmlAttribute] 
        public string ValueFx;
        
        [XmlAttribute] 
        public string DebugName;

        [XmlArrayItem("Param")]
        public FxParamXml[] Params;
    }
}