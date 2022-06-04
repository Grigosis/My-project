using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SecondCycleGame
{
    [RequireComponent(typeof(RectTransform))]
    public class PortraitGroup : MonoBehaviour
    {
        public byte offset = 10;
        public Vector2 portraitSize = new Vector2(80, 100);
        private List<InteractablePortrait> _portraits;
        //public InteractablePortrait draggingPortrait;
        public InteractablePortrait selectedPortrait;

        public Canvas Canvas { get; private set; }

        private void Start()
        {
            _portraits = new List<InteractablePortrait>();
            Canvas = GetComponentInParent<Canvas>();

            foreach (var portrait in transform.GetComponentsInChildren<InteractablePortrait>())
            {
                _portraits.Add(portrait);
                portrait.SetDefaultAnchors();
                portrait.RectTransform.pivot = new Vector2(0.5f, 0.5f);
                portrait.RectTransform.sizeDelta = new Vector2(80, 100);
            }
            RecalculateAll();
        }
        public void RecalculateGroupSize(RectTransform group)
        {
            for (int i = 0; i < group.transform.childCount; i++)
            {
                var rt = group.transform.GetChild(i).GetComponent<RectTransform>();
                var yOffset = (rt.sizeDelta.y / 2) + i * rt.sizeDelta.y + offset * (i + 1);
                rt.anchoredPosition = new Vector2(portraitSize.x/2 + offset, -yOffset);
            }
            group.sizeDelta = new Vector2(group.sizeDelta.x, offset * (group.transform.childCount + 1) + group.transform.childCount * portraitSize.y);
        }
        public void RecalculateAll()
        {
            //RecalculateGroupSize(_group1);
            ////_group1.anchoredPosition = 
            //RecalculateGroupSize(_group2);
        }

        public void RecalcGroupSize(params RectTransform[] groups)
        {
            for (int i = 0; i < groups.Length; i++)
            {

            }
        }
    }
}
