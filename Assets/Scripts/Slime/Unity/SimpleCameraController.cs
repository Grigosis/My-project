using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

namespace Assets.Scripts.Slime.Unity
{
    public class SimpleCameraController : MonoBehaviour
    {
        public Camera Camera;

        private Vector3 m_prevMousePos;
        private bool m_active;
        public float Sensitivity = 8;
        public float ScrollSensitivity = 8;
        
        void Update()
        {
            if (Input.mouseScrollDelta.y != 0)
            {
                Camera.orthographicSize -= Input.mouseScrollDelta.y / ScrollSensitivity;
            }
            if (Input.GetMouseButton(2))
            {

                if (!m_active)
                {
                    m_prevMousePos = Input.mousePosition;
                    m_active = true;
                }
                else
                {
                    var delta = Input.mousePosition - m_prevMousePos;
                    if (Vector3.zero != delta)
                    {
                        delta /= Sensitivity; 
                        m_prevMousePos = Input.mousePosition;
                        m_active = true;
                        var d = new Vector3(delta.x, delta.y, -delta.x);
                        Debug.Log("MouseDown:" + delta + " => " + d);
                        Camera.transform.position += d;
                    }
                }
            }
            else
            {
                m_active = false;
            }
        }
    }
}