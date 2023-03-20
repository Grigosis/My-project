using System;
using System.Collections.Generic;
using System.Linq;
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

        public class PositioningInfo
        {
            public float MoveCost;
            public List<float> Results = new List<float>();
            public bool CanMove = true;
            public StringBuilder DebugFullInfo = new StringBuilder();
            public StringBuilder DebugShortInfo = new StringBuilder();
            public Vector2Int Position;
        }
        
        public void Start(Battle battle, AIController controller)
        {
            var m_controller = battle.BattleUnity.battleMapCellController;
            var m_battle = battle;

            //using (new Measure("AI Positioning: GUI 1"))
            //{
            //    m_controller.ClearAll();
            //}
            
            
            var from = controller.Entity.Position;
            var cells = DI.GetCellsAvailableForMovement(m_battle.BattleMap, from, 30);

            
            
            List<PositioningInfo> infos = new List<PositioningInfo>();
            foreach (var move in cells)
            {
                PositioningInfo info = new PositioningInfo();
                info.Position = move.Position;
                info.MoveCost = move.Cost;
                infos.Add(info);
                
                foreach (var layer in Layers)
                {
                    var value = layer.Do(battle, controller, battle.BattleMap[move.Position], move.Cost);
                    if (value == float.MinValue)
                    {
                        info.DebugFullInfo.AppendLine($"{layer.Xml.DebugName}:{layer.Xml.ValueFx}:CantMoveHere");
                        info.CanMove = false;
                        break;
                    }
                    else
                    {
                        value = Mathf.Clamp(value, layer.Xml.MinValue, layer.Xml.MaxValue);
                        info.Results.Add(value);
                        info.DebugFullInfo.AppendLine($"{layer.Xml.DebugName}:{layer.Xml.ValueFx}:{value}");
                    }
                }
            }

            

            var max = float.MinValue;
            var min = float.MaxValue;
            foreach (var info in infos)
            {
                var sum = info.Results.Sum();
                min = Math.Min(sum, min);
                max = Math.Max(sum, max);
            }

            var green = min + (max - min) * 0.75;
            var yellow = min + (max - min) * 0.50;


            //using (new Measure("AI Positioning: GUI 2"))
            {
                foreach (var info in infos)
                {
                    var sum = info.Results.Sum();

                    Color color;
                    if (!info.CanMove)
                    {
                        color = Color.red;
                    }
                    else
                    {
                        if (sum >= green)
                        {
                            color = Color.green;
                        } else if (sum > yellow)
                        {
                            color = Color.yellow;
                        }
                        else
                        {
                            color = new Color(1f, 0.7f, 0);
                        }
                    }
                
                    m_controller.GetOrCreate(info.Position, from, color, info.CanMove ? $"{sum:F2}" : "", info.DebugFullInfo.ToString());
                }
            }
            
            
            
            m_controller.ClearOther(infos, (x)=>x.Position);
        }

        public void End(Battle battle, AIController controller)
        {
            
        }
    }
}

/*for (var i = 0; i < Layers.Length; i++)
{
    var layer = Layers[i];
    var max = float.MinValue;
    var min = float.MaxValue;


    foreach (var info in infos)
    {
        if (!info.CanMove) continue;
        var result = info.Results[i];
        min = Math.Min(result, min);
        max = Math.Min(result, max);
    }

    foreach (var info in infos)
    {
        if (!info.CanMove) continue;
        var result = info.Results[i];
        min = Math.Min(result, min);
        max = Math.Min(result, max);
    }
}*/