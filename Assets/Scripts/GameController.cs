using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SecondCycleGame
{
    public class GameController : MonoBehaviour
    {
        private Context _context;
        public Unit player;
        public LayerMask Ground;

        void Awake()
        {
            _context = new Context();

            _context.inputs.Controls.LMB.canceled += ctx => OnMoveButtonClick();
            _context.inputs.Controls.Run.performed += ctx => OnRunButtonClick();
            _context.inputs.Controls.Crouch.performed += ctx => OnCrouchButtonClick();
        }
        private void OnEnable()
        {
            _context.inputs.Enable();
        }
        private void OnDisable()
        {
            _context.inputs.Disable();
        }
        void Start()
        {
        
        }
        void Update()
        {
        
        }

        private void OnMoveButtonClick()
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hitinfo, 100, Ground))
            {
                player.MoveToPosition(hitinfo.point);
            }
        }
        private void OnRunButtonClick()
        {
            //player.Run(!player.isRunning);
            player.SetState(player.run);
        }
        private void OnCrouchButtonClick()
        {
            //player.Crouch(!player.isCrouching);
            player.SetState(player.crouch);
        }

    }
}
