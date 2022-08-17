using Assets.Scripts.Slime.Core;
using ROR.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Slime.GUI
{
    public class Portrait : MonoBehaviour
    {
        public Image Image;
        public LivingEntity Entity;
        
        public void Attach(LivingEntity entity)
        {
            Sprite sprite = R.Instance.Load<Sprite> (entity?.Portrait ?? "Icons/Icon3");
            Entity = entity;
            Image.sprite = sprite;
        }
    }
}