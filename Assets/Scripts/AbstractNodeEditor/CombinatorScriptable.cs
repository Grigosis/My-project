using UnityEngine;

namespace Assets.Scripts.AbstractNodeEditor
{
    [CreateAssetMenu(fileName = "New SwordData", menuName = "Sword Data", order = 51)]
    public class CombinatorScriptable : ScriptableObject
    {
        [SerializeField]
        private string swordName;
        [SerializeField]
        private string description;
        [SerializeField]
        private Sprite icon;
        [SerializeField]
        private int goldCost;
        [SerializeField]
        private int attackDamage;
        
        [SerializeField]
        private Vector3 Position;
        
        [SerializeField]
        private CombinatorScriptable parent;
    }
}