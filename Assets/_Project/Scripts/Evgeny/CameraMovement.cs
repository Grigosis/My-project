using UnityEngine.InputSystem;
using UnityEngine;
using Cinemachine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private float _borderSize = 50;
    [SerializeField] private float _wasdSpeed = 10;
    [SerializeField] private float _mouseSpeed = 10;
    private Vector3 _inputMouseVector;
    private Vector3 _inputWASDVector;
    private Vector3 _startPosition;

    private void Start() {
        _startPosition = transform.position;
    }

    public void SetInputVector(InputAction.CallbackContext context) {
        if (context.performed) {
            Vector2 inputVector = context.ReadValue<Vector2>();
            Vector3 direction = new Vector3(inputVector.x, 0f, inputVector.y);
            _inputWASDVector = direction * _wasdSpeed * Time.deltaTime;
        }
        else {
            _inputWASDVector = Vector3.zero;
        }
    }

    private void LateUpdate() {
        if (!WASDMove()) {
            MouseBorderMove();
        }
    }

    private bool WASDMove() {
        if (_inputWASDVector.magnitude > 0.1f) {
            transform.Translate(_inputWASDVector, Space.Self);
            _virtualCamera.transform.position = new Vector3(transform.position.x, _startPosition.y, transform.position.z);
            return true;
        }
        return false;
    }

    private void MouseBorderMove() {
        Vector3 mousePosition = Input.mousePosition;
        _inputMouseVector = Vector3.zero;

        if (mousePosition.x < _borderSize) {
            _inputMouseVector = Vector3.left;
        }
        else if (mousePosition.x > Screen.width - _borderSize) {
            _inputMouseVector = Vector3.right;
        }

        if (mousePosition.y < _borderSize) {
            _inputMouseVector = Vector3.back;
        }
        else if (mousePosition.y > Screen.height - _borderSize) {
            _inputMouseVector = Vector3.forward;
        }

        if (_inputMouseVector.magnitude >= 0.1f) {
            Vector3 movement = new Vector3(_inputMouseVector.x, 0, _inputMouseVector.z) * _mouseSpeed * Time.deltaTime;
            transform.Translate(movement, Space.Self);
            transform.position = new Vector3(transform.position.x, _startPosition.y, transform.position.z);
            _virtualCamera.transform.Translate(movement, Space.Self);
            _virtualCamera.transform.position = new Vector3(transform.position.x, _startPosition.y, transform.position.z);
        }
    }
}