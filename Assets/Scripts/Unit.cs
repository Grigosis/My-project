using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    public LayerMask Ground;
    private NavMeshAgent myAgent;
    [SerializeField] [Range(1, 4)] private float _walkSpeed = 2;
    [SerializeField] [Range(2, 8)] private float _runSpeed = 4;

    private Animator _anim;
    public bool isRunning;
    public bool isMoving;
 
    
    private void Start()
    {
        myAgent = GetComponent<NavMeshAgent>();
        myAgent.speed = 4;
        _anim = GetComponent<Animator>();
    }
    public void Walk()
    {
        if (myAgent.remainingDistance <= myAgent.stoppingDistance)
        {
            SetMove(false);
        }
    }
    private void Update()
    {
        if(isMoving) Walk();
    }
    public void MoveTo(Vector3 position)
    {
        myAgent.SetDestination(position);
        SetMove(true);
    }
    private void SetMove(bool value)
    {
        isMoving = value;
        _anim.SetBool("isMoving", value);
    }
}
