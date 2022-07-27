using System;
using UnityEngine;

namespace Assets.Scripts.Slime.GUI
{
    public class ISelectable : MonoBehaviour
    {
        public bool m_selected = false;
        public bool Selected
        {
            get => m_selected;
            set
            {
                if (value != m_selected)
                {
                    m_selected = value;
                    OnSelected?.Invoke(gameObject);
                }
            }
        }

        public GameObject Selection;

        private event Action<GameObject> OnSelected;

    }
}