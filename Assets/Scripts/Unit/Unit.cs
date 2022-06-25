using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SecondCycleGame
{
    public abstract class Unit<T> where T: UnitData
    {
        public readonly HumanBehaviour behaviour;
        public readonly T data;

        public Unit(T unitData)
        {
            data = unitData;
            behaviour = Object.Instantiate(data.Prefab);
        }
    }
}
