using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using Cinemachine;

namespace SecondCycleGame
{
    public class CameraController : MonoBehaviour
    {
        #region Fields
        private InputActions _inputs;
        private Camera _camera;
        public Vector3 _newPosition;
        [Header("MOVE")]
        [SerializeField] private int _moveAcceleration = 10;
        [SerializeField] [Range(5, 20)] private int _keyboardMoveSpeed = 10;
        private const float MOVE_STOP_AMOUNT = 0.0001f;
        private bool _isMoving;
        [Header("ZOOM")]
        [SerializeField] [Range(25, 60)] private int _maxZoom = 50;
        [SerializeField] [Range(0.01f, 0.2f)] private float _zoomSpeed = 0.1f;
        private const int MIN_ZOOM = 20;
        [SerializeField] [Range(0, 1)] private float _zoomAmount;
        private Vector3 _newZoomPosition;
        public CinemachineVirtualCamera vCamera;

        private readonly Vector3 _cameraForward = new Vector3(1, 0, 1);
        private readonly Vector3 _cameraRight = new Vector3(0.7f, 0, -0.7f);
        public Vector2 inputDir;
        #endregion

        #region Properties
        private bool IsMoveInputsActive => _inputs.Camera.Move.inProgress;
        #endregion

        void Start()
        {
            _newPosition = vCamera.transform.position;
            SetCameraZoom(0.5f);

        }
        void Update()
        {
            if (_isMoving) Move();
            //Zoom();
        }


        public void Initialize(InputActions inputs)
        {
            _inputs = inputs;
            _camera = Camera.main;

            _inputs.Camera.Move.started += ctx => _isMoving = true;
            _inputs.Camera.Zoom.performed += ZoomCamera;
        }

        #region Move
        private void Move()
        {
            KeyboardMove();

            vCamera.transform.position = Vector3.Lerp(vCamera.transform.position, _newPosition, Time.deltaTime * _moveAcceleration);
            if (!IsMoveInputsActive)
            {
                if ((_newPosition - vCamera.transform.position).sqrMagnitude < MOVE_STOP_AMOUNT)
                {
                    _isMoving = false;
                }
            }
        }
        private void KeyboardMove()
        {
            if (_inputs.Camera.Move.inProgress)
            {
                var direction = _inputs.Camera.Move.ReadValue<Vector2>();
                inputDir = direction;
                _newPosition += (_cameraForward * direction.y + _cameraRight * direction.x) * Time.deltaTime * _keyboardMoveSpeed;
            }
        }

        public void MoveTo(Vector3 position, float time)
        {
            _inputs.Camera.Disable();
            _newPosition = position;
            transform.DOMove(position, time).onKill = _inputs.Camera.Enable;
        }
        #endregion

        #region Zoom
        private void ZoomCamera(InputAction.CallbackContext callback)
        {
            SetCameraZoom(_zoomAmount - Mathf.Sign(callback.ReadValue<float>()) * _zoomSpeed);
        }
        private void SetCameraZoom(float value)
        {
            _zoomAmount = Mathf.Clamp01(value);
            _newZoomPosition = Vector3.Lerp(new Vector3(0, MIN_ZOOM, -MIN_ZOOM), new Vector3(0, _maxZoom, -_maxZoom), _zoomAmount);
        }
        private void Zoom()
        {
            _camera.transform.localPosition = Vector3.Lerp(_camera.transform.localPosition, _newZoomPosition, Time.deltaTime * 5);
            //var angle = Mathf.Lerp(10, 45, curve.Evaluate(_zoomAmount));
            //_camera.transform.rotation = Quaternion.Lerp(_camera.transform.rotation, Quaternion.Euler(angle, 0, 0), Time.deltaTime * 5);
        }
        #endregion
    }
}
