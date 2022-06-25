using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SecondCycleGame
{
    [RequireComponent(typeof(RectTransform), typeof(Image))]
    public class InteractablePortrait : MonoBehaviour,
        IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private PortraitGroup portraitGroup;
        private RectTransform _rectTransform;
        private Image _image;
        private Character _character;
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

        public void Initialize(Character character)
        {
            _character = character;
            _image.sprite = character.data.Portrait;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(eventData.clickCount == 2)
            {
                print("camera follow");
            }
            else
            {
                print("select");
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_character.GroupMember.SubGroup != null)
                _character.GroupMember.LeaveSubGroup();

            transform.SetParent(portraitGroup.Canvas.transform);
            SetDragAnchors();
            //portraitGroup.RecalculateAll();
            //portraitGroup.draggingPortrait = this;
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
            transform.SetParent(portraitGroup.transform);
            _image.raycastTarget = true;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            portraitGroup.selectedPortrait = this;
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            portraitGroup.selectedPortrait = null;
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
