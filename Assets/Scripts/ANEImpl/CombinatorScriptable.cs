using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Slime.Core;
using Slime;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.AbstractNodeEditor
{
    [Serializable]
    public class CombinatorScriptable
    {
        [SerializeField]
        public string Value;

        [SerializeField]
        public string Fx;
        
        //var t = F.Functions.Keys.ToList();
        //t.Insert(0, "Constant");
        //Helper.CreateEditorClassSelector(ref someClass.Fx, t.ToArray(), "Function");

        [SerializeReference]
        public List<CombinatorScriptable> Nodes = new List<CombinatorScriptable>();

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
    }
}