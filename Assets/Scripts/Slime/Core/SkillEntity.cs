using System;
using System.Collections.Generic;
using Assets.Scripts.Slime.Core.BattleMap.Logic.Interfaces;
using Assets.Scripts.Slime.Core.BattleMap.UnityWrappers.TargetSelectors;
using Assets.Scripts.Slime.Core.Skills;
using ROR.Core;
using ROR.Core.Serialization;
using SecondCycleGame;
using UnityEngine;

namespace Assets.Scripts.Slime.Core
{
    public class SkillEntity
    {
        public int Cooldown = 5;
        public SkillDefinition Definition;
        public LivingEntity Owner;
        public ISkillImplementation Implementation;
        public ITargetSelector TargetSelector;
        public ISplashProvider SplashProvider;
        public ITargetRanger TargetRanger;
        public IUnityTargetSelector UnityTargetSelector;
        
        
        public SkillEntity(LivingEntity owner, SkillDefinition definition)
        {
            Definition = definition;
            Implementation = R.Instance.CreateInstance<ISkillImplementation>(Definition.Implementation);
            TargetSelector = R.Instance.CreateInstance<ITargetSelector>(Definition.TargetSelector);
            UnityTargetSelector = R.Instance.CreateInstance<IUnityTargetSelector>(Definition.UnityTargetSelector);
            SplashProvider = R.Instance.CreateInstanceOrNull<ISplashProvider>(Definition.SplashProvider);
            TargetRanger = R.Instance.CreateInstanceOrNull<ITargetRanger>(Definition.TargetRanger);
            Owner = owner;
        }

        public int GetMinRange()
        {
            return Definition.MinRange;
        }
        
        public int GetMaxRange()
        {
            return Definition.MaxRange;
        }

        public bool HasGoodTargets(Battle battle, LivingEntity caster, Vector2Int fromPosition)
        {
            try
            {
                List<ISkillTarget> buffer = new List<ISkillTarget>();
                if (TargetRanger == null) return false;
                TargetSelector.GetAllPossibleTargets(battle, caster, this, fromPosition, buffer);
                for (var i = 0; i < buffer.Count; i++)
                {
                    var target = buffer[i];
                    var rating = TargetRanger.RangeTargets(caster, this, battle, target, fromPosition);
                    if (rating > 0)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception e)
            {
                Debug.LogError($"Skill [{this.Definition.Id}] HasGoodTargets failed ");
                throw new Exception($"Skill [{this.Definition.Id}] HasGoodTargets failed ", e);
            }
           
        }
    }
}