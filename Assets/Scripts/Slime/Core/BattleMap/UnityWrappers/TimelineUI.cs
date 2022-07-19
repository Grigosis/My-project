using System.Collections.Generic;
using Assets.Scripts.Slime.GUI;
using Assets.Scripts.Slime.Sugar;
using ROR.Core;
using RPGFight.Core;
using TMPro;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.UI;
using Battle = SecondCycleGame.Battle;

namespace Assets.Scripts.Slime.Core.BattleMap
{
    public class TimelineUI : MonoBehaviour
    {
        public BattleMapUnityWrapper BattleWrapper;
        public Text CurrentFrameText;
        public GameObject Prefab;
        public GameObject Container;
        public GameObject Stretcher;
        public List<TimelineEvent> buffer = new List<TimelineEvent>();
        public float Width;
        public float Gap;


        private bool stop = false;
        public void Update()
        {
            //if (stop) return;
            //stop = true;
            
            var tl = BattleWrapper?.Battle?.Timeline;
            if (tl == null) return;
            
            var txt = "Frame:" + tl.CurrentFrame;
            CurrentFrameText.text = txt;
            //CurrentFrameText.GetComponent<TextMeshPro>().text = txt;

            //
            //throw new NotImplementedException();
            
            foreach (var child in Container.GetAllChildren())
            {
                Destroy(child);
            }

            buffer.Clear();
            tl.Foreach((xx) => buffer.Add(xx), 3*Battle.FramesInTurn+1);

            foreach (var timelineEvent in buffer)
            {
                Debug.Log(timelineEvent.Frame);
            }

            float x = Gap;
            foreach (var timelineEvent in buffer)
            {
                x += Gap;
                var go = Instantiate(Prefab, Container.transform);
                go.transform.localPosition = new Vector3(x + Width/2, 0, 0);
                var portrait = go.GetComponentInChildren<Portrait>();
                portrait.Attach((LivingEntity)timelineEvent.Object);
                //portrait.Source = "";
                x += Width;
            }
            x +=  + Width/2 + Gap;
            
            var trans = Stretcher.GetComponent<Image>();
            trans.rectTransform.sizeDelta = new Vector2(x, trans.rectTransform.sizeDelta.y);
            trans.rectTransform.anchoredPosition = new Vector2(x / 2, 0);
        }

        public void Start()
        {
            
        }
        
        
    }
}