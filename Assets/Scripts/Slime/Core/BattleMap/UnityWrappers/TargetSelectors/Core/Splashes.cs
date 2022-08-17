using System;
using SecondCycleGame;

namespace Assets.Scripts.Slime.Core.BattleMap.UnityWrappers.TargetSelectors
{
    public class SquareSplash : ISplashProvider 
    {
        public void GetSplashCells(Battle m_battle, BattleMapCell from, BattleMapCell to, int range, Action<BattleMapCell> action)
        {
            m_battle.BattleMap.Foreach(to, range, action);
        }
    }
}