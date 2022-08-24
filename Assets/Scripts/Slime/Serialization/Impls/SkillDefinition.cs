using Assets.Scripts.Slime.Core.Algorythms.Data;
using UnityEngine;

namespace ROR.Core.Serialization
{
    [CreateAssetMenu(fileName = "USkill", menuName = "GameItems/Skills/Universal", order = 51)]
    public class SkillDefinition : BaseDefinition
    {
        [SerializeField]
        public ElementAttack[] Attacks;

        [SerializeField]
        [HideInInspector]
        public string TargetSelector;
        
        [SerializeField]
        [HideInInspector]
        public string UnityTargetSelector;
        
        [SerializeField]
        [HideInInspector]
        public string SplashProvider;
        
        [SerializeField]
        [HideInInspector]
        public string TargetRanger;
        
        [SerializeField]
        [HideInInspector]
        public string Implementation;

        

        [SerializeField]
        public RelationShip RelationShipFilter = RelationShip.Any;

        [SerializeField]
        public bool IsRangedAttack;

        public int Targets = 1;
        
        [SerializeField]
        public float HitChanceMlt = 1;

        [SerializeField]
        public int SplashRange = 0;
        
        [SerializeField]
        public int AP = 0;
        
        [SerializeField]
        public int MinRange = 0;
        
        [SerializeField]
        public int MaxRange = 1;
        
        [SerializeField]
        public int Cooldown = 10;
    }
}