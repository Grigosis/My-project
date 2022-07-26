using UnityEngine;

namespace SecondCycleGame
{
    public interface IMouseReceiver
    {
        void OnMouseEnter(GameObject gameObject);
        void OnMouseOver(GameObject gameObject);
        void OnMouseExit(GameObject gameObject);
        void OnMouseDown(GameObject gameObject);
        void OnMouseUp(GameObject gameObject);
    }
}
