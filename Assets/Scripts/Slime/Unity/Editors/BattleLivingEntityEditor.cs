using Assets.Scripts.Slime.Core.BattleMap;
using SecondCycleGame;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Slime.Unity.Editors
{
    [CustomEditor(typeof(BattleLivingEntity))]
    public class BattleLivingEntityEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var ble = (BattleLivingEntity)target;
            if (GUILayout.Button("Reload AI"))
            {
                ble.GetComponentInParent<BattleMapUnityWrapper>().AddAction(() =>
                {
                    ble.ReloadAI();
                });
            }
        }
    }
}