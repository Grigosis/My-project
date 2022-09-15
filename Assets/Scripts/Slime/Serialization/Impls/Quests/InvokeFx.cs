using System;
using System.Collections.Generic;
using Assets.Scripts.AbstractNodeEditor;
using ROR.Core.Serialization.Json;
using UnityEngine;

namespace SecondCycleGame.Assets.Scripts.Slime.Serialization.Impls.Quests
{
    
    /// <summary>
    /// Отвечает за вызов 1 функции
    /// </summary>
    public class InvokeFx : Linkable
    {
        [SerializeField] 
        public string Fx;
        
        [NonSerialized]
        public List<CombinatorData> Combinators = new List<CombinatorData>(); 
        
        [SerializeField]
        private List<string> CombinatorGuids = new List<string>();
        
        
        [HideInInspector]
        public string GUID {  get { return Guid; } set { Guid = value; } }

        [HideInInspector]
        [SerializeField] 
        public string Guid;
        
        public void GetLinks(HashSet<Linkable> links)
        {
            foreach (var fx in Combinators)
            {
                links.Add(fx);
            }
        }

        public void RestoreLinks(ReferenceSerializer dictionary)
        {
            Combinators.Clear();
            foreach (var fx in CombinatorGuids)
            {
                Combinators.Add(dictionary.GetObject<CombinatorData>(fx));
            }
        }

        public void StoreLinks()
        {
            CombinatorGuids.Clear();
            foreach (var fx in Combinators)
            {
                CombinatorGuids.Add(fx.GUID);
            }
        }
    }
}