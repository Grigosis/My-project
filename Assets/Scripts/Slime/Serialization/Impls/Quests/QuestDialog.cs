using System.Collections.Generic;
using System.Linq;
using ClassLibrary1;
using Combinator;
using Slime;
using UnityEditor;
using UnityEngine;

namespace ROR.Core.Serialization
{
    public class QuestDialog : ScriptableObject
    {
        [HideInInspector]
        public CombinatorNodeXml VisibilityCombinator;
        public string Text;
        [HideInInspector]
        public string TextArgsFx;
        [HideInInspector]
        public List<QuestAnswer> Answers = new List<QuestAnswer>(); 
    }
    
    [CustomEditor(typeof(QuestDialog))]
    public class QuestDialogUnityEditor : Editor
    {
        public override void OnInspectorGUI ()
        {
            // Draw the default inspector
            DrawDefaultInspector();
            var someClass = target as QuestDialog;
            
            Helper.CreateEditorClassSelector(ref someClass.TextArgsFx, Library.Instance.QuestionArgsFx.Keys.ToArray(), "TextArgsFx");

            // Save the changes back to the object
            EditorUtility.SetDirty(target);
        }
    }
}