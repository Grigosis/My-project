using System.Linq;
using Assets.Scripts.Slime.Core.Algorythms;
using ClassLibrary1;
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
        
        [HideInInspector]
        public string SelectionFx; // Когда выбрали ответ
        [HideInInspector]
        public QuestDialog NextQuestionDialog; //Следующий вопрос
    }
    
    [CustomEditor(typeof(QuestAnswer))]
    public class QuestAnswerUnityEditor : Editor
    {
        public override void OnInspectorGUI ()
        {
            // Draw the default inspector
            DrawDefaultInspector();
            var someClass = target as QuestAnswer;
            
            Helper.CreateEditorClassSelector(ref someClass.AnswerFx, Library.Instance.AnswerArgsFx.Keys.ToArray(), "Implementation");
            Helper.CreateEditorClassSelector(ref someClass.SelectionFx, Library.Instance.SelectionFx.Keys.ToArray(), "TargetSelector");

            // Save the changes back to the object
            EditorUtility.SetDirty(target);
        }
    }
}