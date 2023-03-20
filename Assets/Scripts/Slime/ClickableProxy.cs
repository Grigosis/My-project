using UnityEngine;

namespace SecondCycleGame
{
    public class ClickableProxy : MonoBehaviour
    {
        public GameObject ClickableReceiver;
        public GameObject ClickableReceiver2;

        private void OnMouseEnter()
        {
            if (ClickableReceiver!=null) (ClickableReceiver.GetComponentInParent<IMouseReceiverProxy>())?.GetMouseReceiver()?.OnMouseEnterProxy(gameObject);
            if (ClickableReceiver2!=null)(ClickableReceiver2?.GetComponentInParent<IMouseReceiverProxy>())?.GetMouseReceiver()?.OnMouseEnterProxy(gameObject);
        }
 
        private void OnMouseOver()
        {
            if (ClickableReceiver!=null) (ClickableReceiver.GetComponentInParent<IMouseReceiverProxy>())?.GetMouseReceiver()?.OnMouseOverProxy(gameObject);
            if (ClickableReceiver2!=null)(ClickableReceiver2?.GetComponentInParent<IMouseReceiverProxy>())?.GetMouseReceiver()?.OnMouseOverProxy(gameObject);
        }

        private void OnMouseExit()
        {
            if (ClickableReceiver!=null) (ClickableReceiver.GetComponentInParent<IMouseReceiverProxy>())?.GetMouseReceiver()?.OnMouseExitProxy(gameObject);
            if (ClickableReceiver2!=null)(ClickableReceiver2?.GetComponentInParent<IMouseReceiverProxy>())?.GetMouseReceiver()?.OnMouseExitProxy(gameObject);
        }

        private void OnMouseDown()
        {
            if (ClickableReceiver!=null) (ClickableReceiver.GetComponentInParent<IMouseReceiverProxy>())?.GetMouseReceiver()?.OnMouseDownProxy(gameObject);
            if (ClickableReceiver2!=null)(ClickableReceiver2?.GetComponentInParent<IMouseReceiverProxy>())?.GetMouseReceiver()?.OnMouseDownProxy(gameObject);
        }

        private void OnMouseUp()
        {
            if (ClickableReceiver!=null) (ClickableReceiver.GetComponentInParent<IMouseReceiverProxy>())?.GetMouseReceiver()?.OnMouseUpProxy(gameObject);
            if (ClickableReceiver2!=null)(ClickableReceiver2?.GetComponentInParent<IMouseReceiverProxy>())?.GetMouseReceiver()?.OnMouseUpProxy(gameObject);
        }
    }
}
