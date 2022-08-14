using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Slime.GUI
{
    public class Draggable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler
    {
        public Canvas Canvas;
        private CanvasGroup canvasGroup;
        private RectTransform rectTransform;

        public DraggableReceiver Receiver;
        public Transform DragParent;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            Receiver = GetComponentInParent<DraggableReceiver>();
            
            if (DragParent == null)
            {
                DragParent = gameObject.transform.parent;
            }
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            gameObject.transform.parent = DragParent;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0.6f;
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            rectTransform.anchoredPosition += eventData.delta / Canvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            ChangeParent();
        }

        public void ChangeParent()
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1f;
            
            gameObject.transform.SetParent(Receiver.AttachGameObject.transform, false);
            rectTransform.anchoredPosition = Receiver.AttachGameObject.GetComponent<RectTransform>().anchoredPosition;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            //Debug.Log("OnPointerDown");
        }

        public void OnDropTo(DraggableReceiver draggableReceiver)
        {
            if (Receiver != null)
            {
                Receiver.Detach(this);
            }

            Receiver = draggableReceiver;
            draggableReceiver.Attach(this);
        }
    }
}