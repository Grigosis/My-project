using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Slime.Core;
using ROR.Core.Serialization;
using SecondCycleGame.Assets.Scripts.ObjectEditor;
using Slime;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.AbstractNodeEditor
{
    [Serializable]
    public class CombinatorScriptable : Linkable
    {
        [SerializeField]
        public string Value;

        [ComboBoxEditor("F.Functions", new []{ "Constant" })]
        [SerializeField]
        public string Fx;

        [HideInInspector]
        [field:NonSerialized]
        public List<CombinatorScriptable> Nodes = new List<CombinatorScriptable>();

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

        public void GetAllChildNodes(HashSet<CombinatorScriptable> set)
        {
            foreach (var node in Nodes)
            {
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
            NodesGuids.Clear();
            foreach (var node in NodesGuids)
            {
                Nodes.Add(dictionary.GetObject<CombinatorScriptable>(node));
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