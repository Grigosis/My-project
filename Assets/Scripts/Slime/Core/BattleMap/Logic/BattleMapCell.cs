﻿using System;
using System.Collections.Generic;
using Assets.Scripts.Slime.Core.Algorythms;
using Assets.Scripts.Slime.Core.BattleMap;
using ROR.Core;
using UnityEngine;

namespace Assets.Scripts.Slime.Core
{
    public class BattleMapCell : SkillTarget
    {
        public const float CellSize = 1f;
        
        public int X;
        public int Y;
        public bool CanStand = true;
        public LivingEntity Entity;
        
        public float MovementCost = 1;

        public MapObject Object;
        public List<MapCellCover> Covers = new List<MapCellCover>();

        public MapCellWrapper UnityCell;

        private Renderer _renderer;
        
        public void Color(Color newColor)
        {
            if (_renderer == null)
            {
                _renderer = UnityCell.GetComponentInChildren<Renderer>();
            }

            if (_renderer.material.color != newColor)
            {
                _renderer.material.color = newColor;
            }
            
        }

        public bool IsVisibleFrom(BattleMapCell cell)
        {
            return true;
        }

        public int DistanceTo(BattleMapCell cell)
        {
            return Math.Abs(X - cell.X) + Math.Abs(Y - cell.Y);
        }
    }
}