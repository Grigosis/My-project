using System;
using System.Collections.Generic;
using Assets.Scripts.Slime.Core.Algorythms;
using Assets.Scripts.Slime.Core.BattleMap.Logic.Interfaces;
using ROR.Core;
using SecondCycleGame;

namespace Assets.Scripts.Slime.Core.BattleMap.UnityWrappers.TargetSelectors
{
    public interface IUnityTargetSelector : IMouseReceiver
    {
        void BeginSelection(BattleMapCellController controller, Battle battle, LivingEntity caster, SkillEntity entity);
        void EndSelection();
        void Update();
        event Action<List<SkillTarget>> OnSelected;
    }
}