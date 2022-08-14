using System.Xml.Serialization;

namespace Assets.Scripts.Slime.Core.Algorythms
{
    public abstract class AIBehaviorTriggerXml
    {
        [XmlAttribute] 
        public string TriggerFx;
        
        [XmlArrayItem("Param")]
        public FxParamXml[] Params;
    }
}