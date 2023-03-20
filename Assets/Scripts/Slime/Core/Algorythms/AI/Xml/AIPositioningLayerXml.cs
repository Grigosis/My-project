using System;
using System.Linq;
using Slime;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Slime.Core.Algorythms
{
    
    #if UNITY_EDITOR

    [CustomPropertyDrawer(typeof(AIPositioningLayerXml))]
    public class HexPointDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 5;
        }
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //base.OnGUI(position, property, label);
            // Find the SerializedProperties by name
            
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            //EditorGUI.BeginProperty(position, label, property);
            
            property.CreateSelector("ValueFx", F.AIFunctions.Keys.ToArray(), ref position, label);
            property.CreateStringEditor("DebugName", ref position, label);
            property.CreateFloatEditor("MinValue", ref position, label);
            property.CreateFloatEditor("MaxValue", ref position, label);

            //EditorGUI.EndProperty();
            
            
            
        }
    }

    #endif
    
    //[CustomEditor(typeof(AIPositioningLayerXml))]
    //public class LivingEntityDefinitionEditor : Editor
    //{
    //    public override void OnInspectorGUI()
    //    {
    //        DrawDefaultInspector();
    //        var someClass = (AIPositioningLayerXml)target;
    //        Helper.CreateEditorClassSelector(ref someClass.ValueFx, typeof(AIBehaviorDefinition), "ValueFx");
    //    }
    //}
    
    [Serializable]
    public class AIPositioningLayerXml
    {
        [HideInInspector]
        public string ValueFx;
        public string DebugName;
        
        public float MinValue;
        public float MaxValue;
        public FxParamXml[] Params;
    }
}