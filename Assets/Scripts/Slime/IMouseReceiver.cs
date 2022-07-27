using UnityEngine;

namespace SecondCycleGame
{
    public interface IMouseReceiverProxy
    {
        IMouseReceiver GetMouseReceiver();
    }
    
    public interface IMouseReceiver
    {
        void OnMouseEnter(GameObject gameObject);
        void OnMouseOver(GameObject gameObject);
        void OnMouseExit(GameObject gameObject);
        void OnMouseDown(GameObject gameObject);
        void OnMouseUp(GameObject gameObject);
    }
}
