using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.AbstractNodeEditor;
using Assets.Scripts.Slime.Core;
using Slime;
using UnityEditor;
using UnityEngine;

namespace ROR.Core.Serialization
{
    public class QuestDialog
    {
        [SerializeReference]
        public CombinatorScriptable VisibilityCombinator;

        [SerializeField]
        public string Id;
        
        [SerializeField]
        public string Text;
        
        [SerializeField]
        public string TextArgsFx; //Helper.CreateEditorClassSelector(ref someClass.TextArgsFx, F.QuestionArgsFx.Keys.ToArray(), "TextArgsFx");
        
        [SerializeReference]
        public List<QuestAnswer> Answers = new List<QuestAnswer>();
    }
}