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
        public Vector3 _newPosition;
        [Header("MOVE")]
        [SerializeField] private int _moveAcceleration = 10;
        [SerializeField] [Range(5, 20)] private int _keyboardMoveSpeed = 10;
        private const float MOVE_STOP_AMOUNT = 0.0001f;
        private bool _isMoving;
        [Header("ZOOM")]
        [SerializeField] [Range(2, 6)] private int _minZoom = 4;
        [SerializeField] [Range(7, 15)] private int _maxZoom = 10;
        [SerializeField] [Range(1, 5)] private float _zoomSpeed = 2;
        private float _zoomAmount;
        private bool _isZooming;

        public CinemachineVirtualCamera vCamera;

        private readonly Vector3 _cameraForward = new Vector3(1, 0, 1);
        private readonly Vector3 _cameraRight = new Vector3(0.7f, 0, -0.7f);
        #endregion

        #region Properties
        private bool IsMoveInputsActive => _inputs.Camera.Move.inProgress;
        private float SpeedZoomModifier => _zoomAmount / _maxZoom;
        #endregion

        void Start()
        {
            _newPosition = vCamera.transform.position;
            //SetCameraZoom(0.5f);
            _zoomAmount = vCamera.m_Lens.OrthographicSize;
        }
        void Update()
        {
            if (_isMoving) Move();
            ZoomUpdate();
        }


        public void Initialize(InputActions inputs)
        {
            _inputs = inputs;

            _inputs.Camera.Move.started += ctx => _isMoving = true;
            _inputs.Camera.Zoom.performed += ctx => CameraZoom(ctx.ReadValue<float>());
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
                _newPosition += (_cameraForward * direction.y + _cameraRight * direction.x) * Time.deltaTime * _keyboardMoveSpeed * SpeedZoomModifier;
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
        public void CameraZoom(float value)
        {
            _zoomAmount = Mathf.Clamp(_zoomAmount - Mathf.Sign(value), _minZoom, _maxZoom);
            _isZooming = true;
        }
        public void ZoomUpdate()
        {
            if (_isZooming)
            {
                vCamera.m_Lens.OrthographicSize = Mathf.Lerp(vCamera.m_Lens.OrthographicSize, _zoomAmount, Time.deltaTime * _zoomSpeed);

                if (Mathf.Abs(_zoomAmount - vCamera.m_Lens.OrthographicSize) < 0.001f)
                {
                    _isZooming = false;
                    vCamera.m_Lens.OrthographicSize = _zoomAmount;
                }
            }
        }
        #endregion
    }
}
