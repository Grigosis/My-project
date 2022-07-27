using UnityEngine;

namespace SecondCycleGame
{
    public class ClickableProxy : MonoBehaviour
    {
        public GameObject ClickableReceiver;

        private void OnMouseEnter()
        {
            (ClickableReceiver.GetComponentInParent<IMouseReceiverProxy>())?.GetMouseReceiver()?.OnMouseEnter(gameObject);
        }
 
        private void OnMouseOver()
        {
            (ClickableReceiver.GetComponentInParent<IMouseReceiverProxy>())?.GetMouseReceiver()?.OnMouseOver(gameObject);
        }

        private void OnMouseExit()
        {
            (ClickableReceiver.GetComponentInParent<IMouseReceiverProxy>())?.GetMouseReceiver()?.OnMouseExit(gameObject);
        }

        private void OnMouseDown()
        {
            (ClickableReceiver.GetComponentInParent<IMouseReceiverProxy>())?.GetMouseReceiver()?.OnMouseDown(gameObject);
        }

        private void OnMouseUp()
        {
            (ClickableReceiver.GetComponentInParent<IMouseReceiverProxy>())?.GetMouseReceiver()?.OnMouseUp(gameObject);
        }
    }
}
