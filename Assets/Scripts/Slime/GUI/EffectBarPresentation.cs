using System;
using System.Collections.Generic;
using SecondCycleGame;
using UnityEngine;
using UnityEngine.UI;

namespace ROR.Core{
    public class EffectBarPresentation : MonoBehaviour{
        public GameObject IconPrefab;
        public BattleLivingEntity Entity;
        public int Line = 6;
        public float IconScale = 0.5f;
        public ChildGravity IconsGravity = ChildGravity.TopLeft;
        
        public EffectBar Bar;
        private List<EffectIcon> icons = new List<EffectIcon>();

        // Start is called before the first frame update
        void Start(){
            Bar = Entity.LivingEntity.EffectBar;
        }

        // Update is called once per frame
        void Update()
        {
            Bar = Entity.LivingEntity.EffectBar;
            int changeLen = Math.Min(Bar.Effects.Data.Count, icons.Count);
            for(int i = 0; i < changeLen; i++){
                icons[i].ChangeTarget(Bar.Effects.Data[i]);
            }
            if(changeLen < Bar.Effects.Data.Count) {
                for(int i = changeLen; i < Bar.Effects.Data.Count; i++){
                    icons.Add(AddIcon(i, Bar.Effects.Data[i]));
                }
            } else if(changeLen < icons.Count){
                for(int i = changeLen; i < icons.Count; i++){
                    DestroyIcon(icons[i]);
                }
                icons.RemoveRange(changeLen, icons.Count - changeLen);
            }
        }

        private EffectIcon AddIcon(int pos, EffectEntity target){
            Debug.LogWarning("AddIcon:"+target);
            GameObject icon = Instantiate(IconPrefab, gameObject.transform);
            Graphic iconG = icon.GetComponent<Graphic>();
            var iconTransform = iconG.rectTransform;
            iconTransform.localScale = new Vector3(IconScale, IconScale, 1);
            Vector2 iconsAnchor = ChildGravityToAnchor(IconsGravity);
            iconTransform.anchorMin = iconsAnchor;
            iconTransform.anchorMax = iconsAnchor;
            int posX = pos % Line;
            int posY = pos / Line;
            float x = iconTransform.sizeDelta.x * IconScale * (posX + iconTransform.pivot.x);
            float y = iconTransform.sizeDelta.y * IconScale * (posY + iconTransform.pivot.y);
            if(iconsAnchor.x != 0){
                x = -x;
            }
            if(iconsAnchor.y != 0){
                y = -y;
            }
            iconTransform.anchoredPosition = new Vector2(x, y);
            icon.GetComponent<EffectIcon>().ChangeTarget(target);
            return icon.GetComponent<EffectIcon>();
        }

        private void DestroyIcon(EffectIcon icon){
            icon.transform.SetParent(null);
            Destroy(icon.gameObject);
        }

        private static Vector2 ChildGravityToAnchor(ChildGravity gravity){
            switch(gravity){
                case ChildGravity.TopLeft: return new Vector2(0, 1);
                case ChildGravity.TopRight: return new Vector2(1, 1);
                case ChildGravity.BottomLeft: return new Vector2(0, 0);
                case ChildGravity.BottomRight: return new Vector2(1, 0);
                default:
                    return new Vector2(0, 1);
            }
        }
    }

    public enum ChildGravity{
        TopLeft = 0, TopRight = 1,
        BottomLeft = 2, BottomRight = 3
    }
}