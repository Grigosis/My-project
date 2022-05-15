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

            _context.inputs.Controls.LMB.canceled += ctx => OnLeftMouseButtonClick();
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

        private void OnLeftMouseButtonClick()
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hitinfo, 100, Ground))
            {
                player.MoveTo(hitinfo.point);
            }
        }
    }
}
