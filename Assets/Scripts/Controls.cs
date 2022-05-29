using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace SecondCycleGame
{
    public sealed class Controls
    {
        private readonly GameController _gameController;
        public readonly InputActions inputs;
        public readonly LayerMask ground;
        private bool _doRelease;
        public bool IsMouseOverUI => EventSystem.current.IsPointerOverGameObject();

        public Controls(GameController gameController)
        {
            _gameController = gameController;
            inputs = new InputActions();
            ground = LayerMask.GetMask("Ground");
            SubscribeOnEvents();
        }

        public void Update()
        {
            LeftMousePress();
            if(_doRelease) LeftMouseRelease();
        }

        private void SubscribeOnEvents()
        {
            inputs.Controls.Run.performed += ctx => OnRunButtonClick();
            inputs.Controls.Crouch.performed += ctx => OnCrouchButtonClick();
        }
        private void UnsubscribeEvents()
        {
            inputs.Controls.Run.performed -= ctx => OnRunButtonClick();
            inputs.Controls.Crouch.performed -= ctx => OnCrouchButtonClick();
        }
        private void LeftMousePress()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                _doRelease = !IsMouseOverUI;
            }
        }
        private void LeftMouseRelease()
        {
            if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                _doRelease = false;
                if(!IsMouseOverUI)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
                    if (Physics.Raycast(ray, out RaycastHit hitinfo, 100, ground))
                    {
                        _gameController.player.MoveToPosition(hitinfo.point);
                    }
                }
            }
        }
        private void OnRunButtonClick()
        {
            _gameController.player.SetMoveType(_gameController.player.run);
        }
        private void OnCrouchButtonClick()
        {
            _gameController.player.SetMoveType(_gameController.player.crouch);
        }

        public void OnTearDown()
        {
            UnsubscribeEvents();
        }
    }
}
