using System.Collections.Generic;
using Assets.Scripts.Slime.Core.Algorythms.Data;
using SecondCycleGame;
using UnityEngine;

namespace Assets.Scripts.Slime.Core.Algorythms.Logic
{
    public class AIPositioningLayer
    {
        public Dictionary<string, FxParam> Params = new Dictionary<string, FxParam>();
        public AIFunction Fx;
        public AIPositioningLayerXml Xml;
        public AIPositioningLayer(AIPositioningLayerXml xml)
        {
            Xml = xml;
            foreach (var par in xml.Params)
            {
                Params.Add(par.Name, new FxParam(par));
            }

            if (!F.AIFunctions.TryGetValue(xml.ValueFx, out Fx))
            {
                Fx = (battle, controller, selector, args) => 66;
                Debug.LogWarning($"Fx [{xml.ValueFx}] NotFound");
            }
        }

        public float Do(Battle battle, AIController controller, BattleMapCell cell)
        {
            return Fx.Invoke(battle, controller, cell, Params);
        }
    }
}