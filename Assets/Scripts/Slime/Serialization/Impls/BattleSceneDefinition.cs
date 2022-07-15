using System.Xml.Serialization;
using RPGFight;

namespace ROR.Core.Serialization
{
    public class LivingStateInBattle
    {
        [XmlAttribute] public int x;
        [XmlAttribute] public int y;
        public Attrs Attributes;
    }

    public class MapState
    {

    }

    public class BattleSceneState
    {
        public MapState Map;

        [XmlArrayItem("Member")]
        public LivingStateInBattle[] Party1;

        [XmlArrayItem("Member")]
        public LivingStateInBattle[] Party2;
    }
}