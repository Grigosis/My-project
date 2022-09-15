using System;
using System.Collections.Generic;
using Assets.Scripts.AbstractNodeEditor;
using ROR.Core.Serialization;
using ROR.Core.Serialization.Json;
using UnityEngine;

namespace SecondCycleGame.Assets.Scripts.Slime.Serialization.Impls.Quests
{
    /// <summary>
    /// Отвечает за вызов множества функций
    /// </summary>
    public class InvokeList  : Linkable
    {
        [HideInInspector]
        [NonSerialized]
        public List<InvokeFx> InvokeFunctions = new List<InvokeFx>();

        [HideInInspector]
        [SerializeField]
        private List<string> InvokeFunctionGuids = new List<string>();
        
        
        [HideInInspector]
        public string GUID {  get { return Guid; } set { Guid = value; } }

        [HideInInspector]
        [SerializeField] 
        public string Guid;
        
        public void GetLinks(HashSet<Linkable> links)
        {
            foreach (var fx in InvokeFunctions)
            {
                links.Add(fx);
            }
        }

        public void RestoreLinks(ReferenceSerializer dictionary)
        {
            InvokeFunctions.Clear();
            foreach (var fx in InvokeFunctionGuids)
            {
                InvokeFunctions.Add(dictionary.GetObject<InvokeFx>(fx));
            }
        }

        public void StoreLinks()
        {
            InvokeFunctionGuids.Clear();
            foreach (var fx in InvokeFunctions)
            {
                InvokeFunctionGuids.Add(fx.GUID);
            }
        }
    }
}