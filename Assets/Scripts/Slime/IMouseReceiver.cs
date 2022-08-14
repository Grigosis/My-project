using UnityEngine;

namespace SecondCycleGame
{
    public interface IMouseReceiverProxy
    {
        IMouseReceiver GetMouseReceiver();
    }
    
    public interface IMouseReceiver
    {
        void OnMouseEnterProxy(GameObject gameObject);
        void OnMouseOverProxy(GameObject gameObject);
        void OnMouseExitProxy(GameObject gameObject);
        void OnMouseDownProxy(GameObject gameObject);
        void OnMouseUpProxy(GameObject gameObject);
    }
}
