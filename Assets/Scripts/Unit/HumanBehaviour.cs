using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SecondCycleGame
{
    [RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
    public class HumanBehaviour : MonoBehaviour
    {
        private NavMeshAgent _agent;
        private Animator _animator;

        [SerializeField] [Range(1, 10)] private int _turnSpeed = 5;

        private bool _isMoving;
        private bool _isRunning;
        private bool _isCrouching;


        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
        }
        private void Start()
        {
            _agent.updateRotation = false;
            _agent.updatePosition = true;
            _agent.speed = 0;
            //myAgent.updateUpAxis = false;
        }
        private void FixedUpdate()
        {
            if (_isMoving)
            {
                DirectionUpdate();
                CheckDistance();
            }
        }

        private void DirectionUpdate()
        {
            var delta = _agent.steeringTarget - transform.position;
            var angle = Vector3.SignedAngle(delta, _agent.transform.forward, Vector3.up);
            angle = Mathf.LerpAngle(0, angle, Time.deltaTime * _turnSpeed);
            transform.Rotate(new Vector3(0, -angle, 0));
        }
        private void CheckDistance()
        {
            if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                SetMove(false);
            }
        }
        public void MoveToPosition(Vector3 position)
        {
            _agent.SetDestination(position);
            SetMove(true);
        }
        private void SetMove(bool value)
        {
            _isMoving = value;
            _animator.SetBool("isMoving", value);
        }
        private void ToggleRun()
        {
            if (_isCrouching) ToggleCrouch();
            _isRunning = !_isRunning;
            _animator.SetBool("isRunning", _isRunning);
        }
        private void ToggleCrouch()
        {
            if (_isRunning) ToggleRun();
            _isCrouching = !_isCrouching;
            _animator.SetBool("isCrouching", _isCrouching);
        }
    }
}