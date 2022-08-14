using System.Text;
using RPGFight;
using SecondCycleGame;
using UnityEngine;

namespace Assets.Scripts.Slime.Core.Algorythms.Logic
{
    public class AIPositioning
    {
        private AIPositioningLayer[] Layers;

        public AIPositioning(AIPositioningXml xml)
        {
            Layers = new AIPositioningLayer[xml.Layers.Length];
            for (var i = 0; i < xml.Layers.Length; i++)
            {
                var layer = xml.Layers[i];
                Layers[i] = new AIPositioningLayer(layer);
            }
        }
        
        public void Start(Battle battle, AIController controller)
        {
            var m_controller = battle.BattleUnity.battleMapCellController;
            var m_battle = battle;
            
            m_controller.ClearAll();
            var from = controller.Entity.Position;
            var cells = DI.GetCellsAvailableForMovement(m_battle.BattleMap, from, 30);

            foreach (var move in cells)
            {
                var sb = new StringBuilder();
                var sum = 0f;
                foreach (var layer in Layers)
                {
                    var value = layer.Do(battle, controller, battle.BattleMap[move.Position]);
                    sum += value;
                    sb.AppendLine(layer.Xml.DebugName + ":" + layer.Xml.ValueFx + ":" + value);
                }

                Debug.LogWarning(sb);
                m_controller.GetOrCreate(move.Position, from, Color.gray, sum.ToString(), sb.ToString());
            }
        }

        public void End(Battle battle, AIController controller)
        {
            
        }
    }
}