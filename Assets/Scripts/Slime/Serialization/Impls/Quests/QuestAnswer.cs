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
    public class QuestAnswer : ScriptableObject
    {
        [HideInInspector]
        public string AnswerFx;
        public string Text; //"Bla lba {MONEY}g? {Nickname}"
        public FxParamXml[] Requirements;

        public CombinatorScriptable CombinatorData;
        
        [HideInInspector]
        public string SelectionFx; // Когда выбрали ответ
        [HideInInspector]
        public QuestDialog NextQuestionDialog; //Следующий вопрос

        public void BuildCombinator(QuestContext context)
        {
            CombinatorBuilder.Build(CombinatorData, typeof(bool), new CombinatorBuilderRules(context, null));
        }
    }
    
    [CustomEditor(typeof(QuestAnswer))]
    public class QuestAnswerUnityEditor : Editor
    {
        public override void OnInspectorGUI ()
        {
            // Draw the default inspector
            DrawDefaultInspector();
            var someClass = target as QuestAnswer;
            
            Helper.CreateEditorClassSelector(ref someClass.AnswerFx, F.AnswerArgsFx.Keys.ToArray(), "Implementation");
            Helper.CreateEditorClassSelector(ref someClass.SelectionFx, F.SelectionFx.Keys.ToArray(), "TargetSelector");

            // Save the changes back to the object
            EditorUtility.SetDirty(target);
        }
    }
}