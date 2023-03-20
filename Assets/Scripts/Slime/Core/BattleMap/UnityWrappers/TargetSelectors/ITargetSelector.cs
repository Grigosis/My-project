using System;
using System.Collections.Generic;
using Assets.Scripts.Slime.Core.BattleMap.Logic.Interfaces;
using ROR.Core;
using SecondCycleGame;
using UnityEngine;

namespace Assets.Scripts.Slime.Core.BattleMap.UnityWrappers.TargetSelectors
{
    public interface ISplashProvider
    {
        void GetSplashCells(Battle m_battle, BattleMapCell from, BattleMapCell to, int range, Action<BattleMapCell> action);
    } 
    
    public interface ITargetSelector
    {
        void GetAllPossibleTargets(Battle m_battle, LivingEntity m_caster, SkillEntity m_skillEntity, Vector2Int fromPosition, List<ISkillTarget> buffer);
    }
    
    public interface ITargetRanger
    {
        double RangeTargets(LivingEntity m_caster, SkillEntity m_skillEntity, Battle m_battle, ISkillTarget target, Vector2Int fromPosition);
    }
}