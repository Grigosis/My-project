﻿using System;
using System.Collections.Generic;
using Assets.Scripts.MyUnity;
using Assets.Scripts.Slime.Core;
using Assets.Scripts.Slime.Core.Algorythms;
using Assets.Scripts.Slime.Core.Algorythms.Data;
using Assets.Scripts.Slime.Core.BattleMap.Logic.Interfaces;
using ROR.Core.Serialization;
using RPGFight;
using RPGFight.Core;
using UnityEngine;
using Battle = SecondCycleGame.Battle;
using Random = System.Random;

namespace ROR.Core
{
    public class LivingEntity : IEffectable, IEntity, ISkillTarget
    {
        public long EntityId { get; set; }

        public int Team;
        public EffectBar EffectBar { get; private set; } = new EffectBar();
        public Sprite Portrait { get; set; }
        public Vector2Int Position => new Vector2Int(Cell.X, Cell.Y);

        public Attrs FinalStats = new Attrs();
        public Attrs SumAttrs = new Attrs();

        public SkillBar SkillBar = new SkillBar();

        private Attrs BaseStats;
        private Attrs UpgradedAttrs;
        private Attrs EquipmentAttrs;
        
        
        public bool IsImmune = false;
        public bool IsDead = false;
        public string DebugName = "";

        public BattleMapCell Cell;
        public Battle Battle;
        public IEntityController Controller;
        public GameObject GameObjectLink;
        public AIController AIController;
        public event Action<LivingEntity> OnDeath;
        
        public LivingEntity()
        {
            FinalStats = new Attrs();
            EffectBar.Init(this);
            Entities.Add(this);
        }

        
        static Random RAND = new Random();
        public void Attach(Battle battle, BattleMapCell cell, int team)
        {
            Battle = battle;
            Cell = cell;
            Team = team;
            //var frame = (int)((1-FinalStats.INITIATIVE.Clamp(0, 0.9999))*Battle.FramesInTurn);
            var frame = (int)(RAND.NextDouble()*Battle.FramesInTurn);
            MoveAt(frame);
            //MoveAt(frame+Battle.FramesInTurn);
            //MoveAt(frame+Battle.FramesInTurn*2);
        }

        public void OnTurnAcquired()
        {
            if (IsDead)
            {
                //TODO Logic of switching
                return;
            }

            FinalStats.EP_NOW = FinalStats.EP_MAX + 3;
            Debug.LogError("EP:" + FinalStats.EP_MAX);
            
            MoveAt(Battle.FramesInTurn);
            SkillBar.Update();
            Battle.SetCurrentPlayer(this);
        }

        public void MoveAt(int deltaFrames)
        {
            Battle.Timeline.Add(deltaFrames, new TimelineEvent(OnTurnAcquired, TimelineActions.PLAYER_TURN, this));
        }

        public void Die()
        {
            IsDead = true;
            OnDeath?.Invoke(this);
            //Destroy(gameObject);
            
            Entities.Remove(this);
        }

        public void Heal(float amount, object from)
        {
            if (amount > 0)
            {
                FinalStats.HP_NOW = Math.Min(FinalStats.HP_NOW + amount, FinalStats.HP_MAX);
                DI.CreateFloatingText(this, $"{amount}", Color.green);
            }
        }

        public void Damage(List<ElementDamage> damages, object from, Random seed)
        {
            for (int i = 0; i < damages.Count; i++)
            {
                Damage(damages[i], from, seed);
            }
        }

        public void Damage(ElementDamage damage, object from, Random seed)
        {
            if (IsImmune) return;
            if (IsDead) return;

            if (damage.Min > 0 && damage.Max > 0)
            {
                var dmg = seed.Between(damage.Min, damage.Max);
                FinalStats.HP_NOW -= dmg;

                var txt = $"{dmg:F0}";
                DI.CreateFloatingText(this, txt, Attrs.GetElementColor(damage.Id));
                
                if (FinalStats.HP_NOW <= 0)
                {
                    Die();
                }
            };
        }


        public void InitFromDefinition(Sprite portrait, Attrs baseAttrs, Attrs upgradedAttrs, Attrs equipmentAttrs, SkillDefinition[] skillDefinitions)
        {
            Portrait = portrait;
            BaseStats = baseAttrs;
            UpgradedAttrs = upgradedAttrs;
            EquipmentAttrs = equipmentAttrs;
            

            SumAttrs.Sum(baseAttrs);
            SumAttrs.Sum(upgradedAttrs);
            SumAttrs.Sum(equipmentAttrs);
            
            FinalStats = new Attrs(SumAttrs);
            FinalStats.CalculateFinalValues();


            SkillBar = new SkillBar();//SkillEntity[skillDefinitions.Length];
            for (var i = 0; i < skillDefinitions.Length; i++)
            {
                var sd = skillDefinitions[i];
                var se = new SkillEntity(this, sd);
                SkillBar.Add(se);
            }
        }

        public void ApplyAttrs()
        {
            FinalStats = new Attrs(SumAttrs);
            FinalStats.CalculateFinalValues();
        } 
        
        public override string ToString()
        {
            return $"LivingEntity{EntityId}/{DebugName} Dead={IsDead} Immune={IsImmune}\r\n{FinalStats}";
        }

        public float GetMaxMoveDistance()
        {
            return 10;
        }

        public RelationShip GetRelationTo(LivingEntity forEntity)
        {
            return this.Team != forEntity.Team ? RelationShip.Enemy : RelationShip.Ally;
        }
        
        public bool IsEnemyTo(LivingEntity forEntity)
        {
            return this.Team != forEntity.Team;
        }
        
        public bool IsAllyTo(LivingEntity forEntity)
        {
            return this.Team == forEntity.Team;
        }
    }
}