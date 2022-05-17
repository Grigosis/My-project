using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SecondCycleGame
{
    public class Unit : MonoBehaviour
    {
        public LayerMask Ground;
        private NavMeshAgent myAgent;
        [SerializeField] [Range(1, 4)] private float _walkSpeed = 2;
        [SerializeField] [Range(1, 10)] private int _turnSpeed = 5;

        private Animator _anim;

        private MoveState _currentState;
        public MoveState run;
        public MoveState crouch;

        public bool isMoving;

        private void Start()
        {
            myAgent = GetComponent<NavMeshAgent>();
            myAgent.speed = _walkSpeed;
            _anim = transform.Find("Model").GetComponent<Animator>();

            myAgent.updateRotation = false;
        }
        private void Update()
        {
            if (isMoving)
            {
                var delta = myAgent.steeringTarget - transform.position;
                var angle = Vector3.SignedAngle(delta, myAgent.transform.forward, Vector3.up);
                angle = Mathf.LerpAngle(0, angle, Time.deltaTime * _turnSpeed);
                transform.Rotate(new Vector3(0, -angle, 0));
                Move();
            }
        }

        public void Move()
        {
            if (myAgent.remainingDistance <= myAgent.stoppingDistance)
            {
                SetMove(false);
            }
        }
        public void MoveToPosition(Vector3 position)
        {
            myAgent.SetDestination(position);
            SetMove(true);
        }
        private void SetMove(bool value)
        {
            isMoving = value;
            _anim.SetBool("isMoving", value);
        }
        public void SetState(MoveState state)
        {
            if(_currentState == state)
            {
                myAgent.speed = _walkSpeed;
                _anim.SetBool(_currentState.animatorBoolName, false);
                _currentState = null;
                return;
            }
            if(_currentState != null)
            {
                _anim.SetBool(_currentState.animatorBoolName, false);
            }
            _currentState = state;
            myAgent.speed = state.speed;
            _anim.SetBool(_currentState.animatorBoolName, true);
        }
    }
}