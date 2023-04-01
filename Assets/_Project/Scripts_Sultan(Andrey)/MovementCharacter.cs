using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;


namespace SecondCycleGame
{
    public class MovementCharacter : MonoBehaviour, IAnimated, IClickResult
    {
        #region PARAMTERS
        [SerializeField] private float _moveSpeed;
        [SerializeField] private LayerMask _floorLayerMask;

        private bool _isWalking = false;
        
        private Animator _animator;
        private NavMeshPath _path;
        
        private float _turnSmoothVelocity = 0.1f;
        private float _turnSmoothTime = 0.1f;
        
        private Vector2 screenPosition;
        
#endregion

        void Start()
        {
            _path = new NavMeshPath();
            _animator = GetComponent<Animator>();
        }

        public bool OnUIClick { get; set; }
        public bool IsObjectClicked { get; set; }

        public void SetBool(string animationName, bool trueOrFalse) => _animator.SetBool(animationName, trueOrFalse);

        public void SetFloat(string animationName, float value) => _animator.SetFloat(animationName, value);

        public void SetInteger(string animationName, int value) => _animator.SetInteger(animationName, value);

        public void SetTrigger(string animationName) => _animator.SetTrigger(animationName);

        public bool UIClicked()
        {
            throw new System.NotImplementedException();
        }

        public void MousePosition(InputAction.CallbackContext cntx) => screenPosition = cntx.ReadValue<Vector2>();

        public void ObjectClick(InputAction.CallbackContext cntx)
        {
            if(_isWalking) return;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(screenPosition), out RaycastHit hit, 1000, _floorLayerMask))
            {
                Vector3 target = hit.point;
                if (NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, _path))
                {
                    _isWalking = true;
                    StartCoroutine(Walking());
                }
            }
        }
        
        public void StopWalking(InputAction.CallbackContext cntx)
        {
            _isWalking = false;
        }

        private IEnumerator Walking()
        {
            
            for(int i = 0; i < _path.corners.Length; i++)
            {
                while (Vector3.Distance(transform.position, _path.corners[i]) > 0.07f)
                {
                    if(!_isWalking) break;

                    Vector3 targetVector = Vector3.MoveTowards(transform.position, _path.corners[i], _moveSpeed * Time.deltaTime);

                    Vector3 direction = targetVector - transform.position;

                    float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);

                    transform.rotation = Quaternion.Euler(transform.eulerAngles.x, angle, transform.eulerAngles.z);

                    transform.position = targetVector;
                    yield return null;
                }
            }
            _isWalking = false;
        }
    }
}
