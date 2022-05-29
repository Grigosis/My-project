using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace SecondCycleGame
{
    public class GameController : MonoBehaviour
    {
        private static GameController _instance;
        private Context _context;
        private Controls _controls; 
        public Unit player;
        public CameraController cameraController;

        void Awake()
        {
            if (_instance == null) _instance = this;
            else
            {
                Destroy(this);
                return;
            }

            _context = new Context();
            _controls = new Controls(this);

            cameraController.Initialize(_controls.inputs);
        }
        private void OnEnable()
        {
            _controls.inputs.Enable();
        }
        private void OnDisable()
        {
            _controls?.inputs.Disable();
        }
        private void OnDestroy()
        {
            _controls?.OnTearDown();
        }
        void Start()
        {
        
        }
        void Update()
        {
            _controls.Update();
        }
    }
}
