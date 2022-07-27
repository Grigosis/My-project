using System.Collections.Generic;
using Assets.Scripts.Slime.Core;
using SecondCycleGame;
using UnityEngine;

namespace ROR.Core
{
    public class SkillBarUI : MonoBehaviour
    {
        public BattleLivingEntity Entity;
        // Start is called before the first frame update
        void Start()
        {

        }
        
        void Update()
        {
            var btns = GetComponentsInChildren<SkillBtn>();
            var skillEntities = Entity?.LivingEntity?.SkillBar.SkillEntities;
            skillEntities ??= new List<SkillEntity>();
            
            for (var i = 0; i < btns.Length; i++)
            {
                if (i >= skillEntities.Count)
                {
                    btns[i].Skill = null; 
                }
                else
                {
                    btns[i].Skill = skillEntities[i]; 
                }
            }

        }
    }
}
