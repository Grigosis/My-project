using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace SecondCycleGame
{
    public class GameController : MonoBehaviour
    {
        private static GameController s_instance;
        private Context _context;
        public CameraController cameraController;
        private PlayersGroup _playersGroup;
        public Transform groupUI;

        void Awake()
        {
            if (s_instance == null) s_instance = this;
            else
            {
                Destroy(this);
                return;
            }

            _context = new Context();
            _playersGroup = new PlayersGroup(groupUI);
        }
        private void OnEnable()
        {
            Controls.inputs.Enable();
        }
        private void OnDisable()
        {
            Controls.inputs.Disable();
        }
        private void OnDestroy()
        {
            if(s_instance == this)
            {
                s_instance = null;
            }
        }
        void Start()
        {
        }
        void Update()
        {
            Controls.Update();
        }
    }
}
