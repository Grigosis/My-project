using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Assets.Scripts.AbstractNodeEditor;
using Assets.Scripts.Slime.Core;
using Assets.Scripts.Slime.Core.Algorythms;
using Combinator;
using ROR.Core.Serialization.Json;
using SecondCycleGame.Assets.Scripts.AbstractNodeEditor;
using SecondCycleGame.Assets.Scripts.ANEImpl.Impls;
using SecondCycleGame.Assets.Scripts.ObjectEditor;
using Slime;
using UnityEditor;
using UnityEngine;

namespace ROR.Core.Serialization
{
    public class QuestAnswer : Linkable
    {
        [SerializeField]
        [ComboBoxEditor("F.AnswerArgsFx")]
        public string AnswerFx;
        
        [Multiline]
        [SerializeField]
        public string Text;
        
        [SerializeField]
        public FxParamXml[] Requirements;

        [HideInInspector]
        [field:NonSerialized]
        public CombinatorData CombinatorData;

        [SerializeField]
        [ComboBoxEditor("F.SelectionFx")]
        public string SelectionFx; // Когда выбрали ответ

        [HideInInspector]
        [field:NonSerialized] 
        public QuestDialog NextQuestionDialog;

        
        [HideInInspector]
        [SerializeField]
        public string NextQuestionDialogGuid;

        [HideInInspector]
        [SerializeField]
        public string CombinatorDataGuid;
        
        public void BuildCombinator(QuestContext context)
        {
            CombinatorBuilder.Build(CombinatorData, typeof(bool), new CombinatorBuilderRules(context, null));
        }

        public override string ToString()
        {
            return $"QuestAnswer [{Text} Next={ (NextQuestionDialog == null ? "NULL" : "Q"+ NextQuestionDialog.GetHashCode()) }]";
        }

        
        [HideInInspector]
        public string GUID {  get { return Guid; } set { Guid = value; } }

        [HideInInspector]
        [SerializeField] 
        public string Guid;
        
        public void GetLinks(HashSet<Linkable> links)
        {
            links.Add(CombinatorData);
            links.Add(NextQuestionDialog);
        }

        public void RestoreLinks(ReferenceSerializer dictionary)
        {
            CombinatorData = dictionary.GetObject<CombinatorData>(CombinatorDataGuid);
            NextQuestionDialog = dictionary.GetObject<QuestDialog>(NextQuestionDialogGuid);
        }

        public void StoreLinks()
        {
            CombinatorDataGuid = CombinatorData?.GUID;
            NextQuestionDialogGuid = NextQuestionDialog?.GUID;
        }
    }
}