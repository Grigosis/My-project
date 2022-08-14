using System.Xml.Serialization;

namespace Assets.Scripts.Slime.Core.Algorythms
{
    public class AIPositioningXml
    {
        [XmlArrayItem("Layer")]
        public AIPositioningLayerXml[] Layers;
    }
}