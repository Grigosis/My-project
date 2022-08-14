using System.Collections.Generic;
using Assets.Scripts.Slime.Core.Algorythms;
using Assets.Scripts.Slime.Core.Algorythms.Data;
using Assets.Scripts.Slime.Core.BattleMap.Logic;
using SecondCycleGame;

namespace Assets.Scripts.Slime.Core
{
    public delegate float AIFunction(Battle battle, AIController controller, BattleMapCell targetSelector, Dictionary<string, FxParam> args);
}