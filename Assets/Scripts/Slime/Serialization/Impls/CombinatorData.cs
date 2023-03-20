using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Slime.Core;
using ROR.Core.Serialization;
using ROR.Core.Serialization.Json;
using SecondCycleGame.Assets.Scripts.ObjectEditor;
using Slime;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.AbstractNodeEditor
{
    [Serializable]
    public class CombinatorData : Linkable
    {
        [SerializeField]
        public string Value;

        [ComboBoxEditor("F.Functions", new []{ "Constant" })]
        [SerializeField]
        public string Fx;

        [HideInInspector]
        [field:NonSerialized]
        public List<CombinatorData> Nodes = new List<CombinatorData>();

        [HideInInspector]
        [SerializeField] 
        public List<string> NodesGuids = new List<string>();

        public override string ToString()
        {
            var s = "";
            foreach (var node in Nodes)
            {
                if (node == null)
                {
                    s += " (NULL)";
                }
                else
                {
                    s += $" ({Fx}:{Value})";
                }
            }
            return $"CombinatorScriptable {Fx} Nodes[{s} ]";
        }

        public void GetAllChildNodes(HashSet<CombinatorData> set)
        {
            foreach (var node in Nodes)
            {
                if (node == null) continue;
                if (set.Add(node))
                {
                    node.GetAllChildNodes(set);
                }
            }
        }

        [HideInInspector]
        public string GUID {  get { return Guid; } set { Guid = value; } }

        [HideInInspector]
        [SerializeField] 
        public string Guid;
        
        public void GetLinks(HashSet<Linkable> links)
        {
            foreach (var node in Nodes)
            {
                links.Add(node);
            }
        }

        public void RestoreLinks(ReferenceSerializer dictionary)
        {
            Nodes.Clear();
            foreach (var node in NodesGuids)
            {
                Nodes.Add(dictionary.GetObject<CombinatorData>(node));
            }
        }

        public void StoreLinks()
        {
            NodesGuids.Clear();
            foreach (var node in Nodes)
            {
                NodesGuids.Add(node.GUID);
            }
        }
    }
}