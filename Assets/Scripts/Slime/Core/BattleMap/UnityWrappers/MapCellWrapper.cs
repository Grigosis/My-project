using SecondCycleGame;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Slime.Core.BattleMap
{
    public class MapCellWrapper : MonoBehaviour, IMouseReceiverProxy, IMouseReceiver
    {
        public int X;
        public int Y;
        public string Debug;
        
        private bool m_over = false;
        
        public string Text
        {
            set
            {
                if (!m_over)
                {
                    ShortInfo.enabled = true;
                } 
                ShortInfo.text = value;
            }
        }

        public string Text2
        {
            set => DetailedInfo.text = value; 
        }
        
        public Vector2Int from;

        public TextMeshPro ShortInfo;
        public TextMeshPro DetailedInfo;

        public UnityEngine.UI.Button Butt;
        

        IMouseReceiver IMouseReceiverProxy.GetMouseReceiver()
        {
            return this;
        }

        public void OnMouseEnterProxy(GameObject gameObject)
        {
            m_over = true;
            ShortInfo.enabled = false;
            DetailedInfo.enabled = true;
        }
        
        public void OnMouseExitProxy(GameObject gameObject)
        {
            m_over = false;
            ShortInfo.enabled = true;
            DetailedInfo.enabled = false;
        }

        public void OnMouseOverProxy(GameObject gameObject) { }
        public void OnMouseDownProxy(GameObject gameObject) { }
        public void OnMouseUpProxy(GameObject gameObject) { }
    }
}