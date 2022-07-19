using Assets.Scripts.Slime.Core.BattleMap;
using UnityEngine;

namespace SecondCycleGame
{
    public class ClickableProxy : MonoBehaviour
    {
        public GameObject ClickableReceiver;

        private void OnMouseEnter()
        {
            (ClickableReceiver.GetComponent<IMouseReceiver>())?.OnMouseEnter(gameObject);
        }
 
        private void OnMouseOver()
        {
            (ClickableReceiver.GetComponent<IMouseReceiver>())?.OnMouseOver(gameObject);
        }

        private void OnMouseExit()
        {
            (ClickableReceiver.GetComponent<IMouseReceiver>())?.OnMouseExit(gameObject);
        }

        private void OnMouseDown()
        {
            (ClickableReceiver.GetComponent<IMouseReceiver>())?.OnMouseDown(gameObject);
        }

        private void OnMouseUp()
        {
            (ClickableReceiver.GetComponent<IMouseReceiver>())?.OnMouseUp(gameObject);
        }
    }
}
