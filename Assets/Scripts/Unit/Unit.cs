using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SecondCycleGame
{
    public abstract class Unit<T> where T: UnitData
    {
        public readonly UnitModel model;
        public readonly T data;

        public Unit(T unitData)
        {
            data = unitData;
            model = Object.Instantiate(data.Prefab).GetComponent<UnitModel>();
        }
    }
}
