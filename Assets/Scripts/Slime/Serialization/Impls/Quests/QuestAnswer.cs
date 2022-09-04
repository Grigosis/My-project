using System;
using System.Linq;
using Assets.Scripts.AbstractNodeEditor;
using Assets.Scripts.Slime.Core;
using Assets.Scripts.Slime.Core.Algorythms;
using Combinator;
using SecondCycleGame.Assets.Scripts.AbstractNodeEditor;
using SecondCycleGame.Assets.Scripts.ANEImpl.Impls;
using Slime;
using UnityEditor;
using UnityEngine;

namespace ROR.Core.Serialization
{
    [Serializable]
    public class QuestAnswer : ScriptableObject
    {
        [SerializeField]
        public string AnswerFx; //Helper.CreateEditorClassSelector(ref someClass.AnswerFx, F.AnswerArgsFx.Keys.ToArray(), "Implementation");
        
        [SerializeField]
        public string Text;
        
        [SerializeField]
        public FxParamXml[] Requirements;

        [SerializeReference]
        public CombinatorScriptable CombinatorData; 
        
        [SerializeField]
        public string SelectionFx; // Когда выбрали ответ
        //Helper.CreateEditorClassSelector(ref someClass.SelectionFx, F.SelectionFx.Keys.ToArray(), "TargetSelector");
        
        [SerializeReference]
        public QuestDialog NextQuestionDialog; //Следующий вопрос

        public void BuildCombinator(QuestContext context)
        {
            CombinatorBuilder.Build(CombinatorData, typeof(bool), new CombinatorBuilderRules(context, null));
        }
    }
}