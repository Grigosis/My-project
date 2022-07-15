using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ROR.Core{
    public class EffectBarPresentation : MonoBehaviour{
        public GameObject iconPrefab;
        public LivingEntity target;
        public EffectBar bar;
        public int line = 6;
        public ChildGravity iconsGravity = ChildGravity.TopLeft;
        public float iconScale = 0.5f;
        private List<EffectIcon> icons = new List<EffectIcon>();

        // Start is called before the first frame update
        void Start(){
            if(bar == null && target != null){
                bar = target.EffectBar;
            }    
        }

        // Update is called once per frame
        void Update()
        {
            if(bar == null && target != null){
                bar = target.EffectBar;
            }

            int changeLen = Math.Min(bar.Effects.Data.Count, icons.Count);
            for(int i = 0; i < changeLen; i++){
                icons[i].ChangeTarget(bar.Effects.Data[i]);
            }
            if(changeLen < bar.Effects.Data.Count) {
                for(int i = changeLen; i < bar.Effects.Data.Count; i++){
                    icons.Add(AddIcon(i, bar.Effects.Data[i]));
                }
            } else if(changeLen < icons.Count){
                for(int i = changeLen; i < icons.Count; i++){
                    DestroyIcon(icons[i]);
                }
                icons.RemoveRange(changeLen, icons.Count - changeLen);
            }
        }

        private EffectIcon AddIcon(int pos, EffectEntity target){
            GameObject icon = Instantiate(iconPrefab, gameObject.transform);
            Graphic iconG = icon.GetComponent<Graphic>();
            var iconTransform = iconG.rectTransform;
            iconTransform.localScale = new Vector3(iconScale, iconScale, 1);
            Vector2 iconsAnchor = ChildGravityToAnchor(iconsGravity);
            iconTransform.anchorMin = iconsAnchor;
            iconTransform.anchorMax = iconsAnchor;
            int posX = pos % line;
            int posY = pos / line;
            float x = iconTransform.sizeDelta.x * iconScale * (posX + iconTransform.pivot.x);
            float y = iconTransform.sizeDelta.y * iconScale * (posY + iconTransform.pivot.y);
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