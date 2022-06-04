using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SecondCycleGame
{
    public class UnitModel : MonoBehaviour
    {
        private NavMeshAgent myAgent;
        private Animator _anim;

        [SerializeField] [Range(1, 10)] private int _turnSpeed = 5;

        public bool isMoving;
        public bool isRunning;
        public bool isCrouching;

        public Unit<UnitData> Unit { get; private set; }

        private void Awake()
        {
            myAgent = GetComponent<NavMeshAgent>();
            _anim = GetComponent<Animator>();
        }
        private void Start()
        {
            myAgent.updateRotation = false;
            myAgent.updatePosition = true;
            myAgent.speed = 0;
            //myAgent.updateUpAxis = false;
        }
        private void FixedUpdate()
        {
            if (isMoving)
            {
                DirectionUpdate();
                CheckDistance();
            }
        }

        public void Initialize(Unit<UnitData> unit)
        {
            if (Unit == null) Unit = unit;
        }
        private void DirectionUpdate()
        {
            var delta = myAgent.steeringTarget - transform.position;
            var angle = Vector3.SignedAngle(delta, myAgent.transform.forward, Vector3.up);
            angle = Mathf.LerpAngle(0, angle, Time.deltaTime * _turnSpeed);
            transform.Rotate(new Vector3(0, -angle, 0));
        }
        private void CheckDistance()
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
        public void ToggleRun()
        {
            if (isCrouching) ToggleCrouch();
            isRunning = !isRunning;
            _anim.SetBool("isRunning", isRunning);
        }
        public void ToggleCrouch()
        {
            if (isRunning) ToggleRun();
            isCrouching = !isCrouching;
            _anim.SetBool("isCrouching", isCrouching);
        }
    }
}