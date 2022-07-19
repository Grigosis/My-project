using ROR.Core;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

namespace Assets.Scripts.Slime.GUI
{
    public class Portrait : MonoBehaviour
    {
        public Image Image;
        
        
        public LivingEntity Entity;
        
        public void Attach(LivingEntity entity)
        {
            Sprite FULLHP = null;
            if (entity == null)
            {
                FULLHP = Load ("Icons/Icon3");
            }
            else
            {
                Entity = entity;
                FULLHP = Load (entity.Portrait);
            }
            
            Image.sprite = FULLHP;
            
        }

        private static Sprite Load(string path)
        {
            Sprite FULLHP2 =  Resources.Load <Sprite>(path);
            if (FULLHP2 == null)
            {
                Debug.LogError("Not loaded2:" + path);
            }

            return FULLHP2;
        } 
    }
}