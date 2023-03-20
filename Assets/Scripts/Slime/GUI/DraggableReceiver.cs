using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Slime.GUI
{
    public interface IDraggableHandler
    {
        bool TryAttach(DraggableReceiver old, DraggableReceiver neww, Draggable draggable);
    }

    
    
    public class DraggableReceiver : MonoBehaviour, IDropHandler
    {
        [SerializeField]
        private Image Image;
        public GameObject AttachGameObject;
        public Draggable Draggable;
        public string DebugName;
        
        

        void Awake()
        {
            Draggable = AttachGameObject.GetComponentInChildren<Draggable>();
            Image.raycastTarget = Draggable == null;
        }

        public void OnDrop(PointerEventData eventData)
        {
            Debug.Log("OnDrop:"+DebugName);
            var draggable = eventData.pointerDrag.GetComponentInChildren<Draggable>();
            if (draggable != null)
            {
                var handlers = GetComponentsInParent<IDraggableHandler>();
                if (handlers.Length == 0)
                {
                    Debug.LogError("No `IDraggableHandler`, to handle this drag");
                    return;
                }
                
                foreach (var handler in handlers)
                {
                    if (handler.TryAttach(draggable.Receiver, this, draggable))
                    {
                        return;
                    }
                }
                
                Debug.LogError("No `IDraggableHandler`, that can handle this drag");
            }
        }
        
        public void Attach(Draggable draggable)
        {
            Draggable = draggable;
            Image.raycastTarget = false;
            Debug.Log("Attach:"+DebugName);
        }

        public void Detach(Draggable draggable)
        {
            Draggable = null;
            Image.raycastTarget = true;
            Debug.Log("Detach:"+DebugName);
        }
    }
}