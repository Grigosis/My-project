using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SecondCycleGame
{
    [RequireComponent(typeof(RectTransform), typeof(Image))]
    public class Portrait : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler
    {
        private PortraitGroup portraitGroup;
        private RectTransform _rectTransform;
        private Image _image;
        public RectTransform RectTransform => _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _image = GetComponent<Image>();
        }
        void Start()
        {
            portraitGroup = GetComponentInParent<PortraitGroup>();
        }
        void Update()
        {        
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(eventData.clickCount == 2)
            {
                print("camera follow");
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            transform.SetParent(portraitGroup.canvas.transform);
            SetDragAnchors();
            portraitGroup.RecalculateAll();
            portraitGroup.draggingPortrait = this;
            _image.raycastTarget = false;
        }
        public void OnDrag(PointerEventData eventData)
        {
            var position = eventData.position;
            position.x = Mathf.Clamp(position.x, 0 + _rectTransform.sizeDelta.x / 2, Screen.width - _rectTransform.sizeDelta.x / 2);
            position.y = Mathf.Clamp(position.y, 0 + _rectTransform.sizeDelta.y / 2, Screen.height - _rectTransform.sizeDelta.y / 2);
            _rectTransform.anchoredPosition = position;
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            portraitGroup.draggingPortrait = null;
            _image.raycastTarget = true;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            print("enter");
            if(portraitGroup.draggingPortrait != null && portraitGroup.draggingPortrait != this)
            {

            }
        }

        public void SetDefaultAnchors()
        {
            _rectTransform.anchorMax = Vector2.up;
            _rectTransform.anchorMin = Vector2.up;
        }
        private void SetDragAnchors()
        {
            _rectTransform.anchorMax = Vector2.zero;
            _rectTransform.anchorMin = Vector2.zero;
        }

    }
}
