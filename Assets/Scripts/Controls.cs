using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace SecondCycleGame
{
    public static class Controls
    {
        public static readonly InputActions inputs;
        private static readonly LayerMask _Ground;
        private static bool _doRelease;
        public static bool IsMouseOverUI => EventSystem.current.IsPointerOverGameObject();
        public static Action<Vector3> OnGroundClick;

        static Controls()
        {
            inputs = new InputActions();
            _Ground = LayerMask.GetMask("Ground");
        }

        public static void Update()
        {
            LeftMousePress();
            if(_doRelease) LeftMouseRelease();
        }

        private static void LeftMousePress()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                _doRelease = !IsMouseOverUI;
            }
        }
        private static void LeftMouseRelease()
        {
            if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                _doRelease = false;
                if(!IsMouseOverUI)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
                    if (Physics.Raycast(ray, out RaycastHit hitinfo, 100, _Ground))
                    {
                        OnGroundClick?.Invoke(hitinfo.point);
                    }
                }
            }
        }
    }
}
