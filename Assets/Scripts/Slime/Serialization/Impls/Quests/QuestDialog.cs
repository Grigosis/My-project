using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.AbstractNodeEditor;
using Assets.Scripts.Slime.Core;
using Slime;
using UnityEditor;
using UnityEngine;

namespace ROR.Core.Serialization
{
    public class QuestDialog : ScriptableObject
    {
        [HideInInspector]
        public CombinatorScriptable VisibilityCombinator;

        public string Id;
        
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
            
            Helper.CreateEditorClassSelector(ref someClass.TextArgsFx, F.QuestionArgsFx.Keys.ToArray(), "TextArgsFx");

            // Save the changes back to the object
            EditorUtility.SetDirty(target);
        }
    }
}