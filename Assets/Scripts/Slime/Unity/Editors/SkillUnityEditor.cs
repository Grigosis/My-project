using Assets.Scripts.Slime.Core.BattleMap.UnityWrappers.TargetSelectors;
using Assets.Scripts.Slime.Core.Skills;
using ROR.Core.Serialization;
using Slime;
using UnityEditor;

namespace Assets.Scripts.Slime.Unity.Editors
{
    [CustomEditor(typeof(SkillDefinition))]
    public class SkillUnityEditor : Editor
    {
        public override void OnInspectorGUI ()
        {
            // Draw the default inspector
            DrawDefaultInspector();
            var someClass = target as SkillDefinition;
            
            Helper.CreateEditorClassSelector(ref someClass.Implementation, typeof(ISkillImplementation), "Implementation");
            Helper.CreateEditorClassSelector(ref someClass.TargetSelector, typeof(ITargetSelector), "TargetSelector");
            Helper.CreateEditorClassSelector(ref someClass.UnityTargetSelector, typeof(IUnityTargetSelector), "UnityTargetSelector");
            Helper.CreateEditorClassSelector(ref someClass.SplashProvider, typeof(ISplashProvider), "SplashProvider");
            Helper.CreateEditorClassSelector(ref someClass.TargetRanger, typeof(ITargetRanger), "TargetRanger");
            
            // Save the changes back to the object
            EditorUtility.SetDirty(target);
        }
    }
}