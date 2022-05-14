using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public LayerMask Ground;
    private NavMeshAgent myAgent;
    public Animator anim;
    public bool isRunning;
    public bool isMoving;
 
    
    private void Start()
    {
        myAgent = GetComponent<NavMeshAgent>();
    }
    public void Run()
    {
        if (myAgent.remainingDistance <= myAgent.stoppingDistance)
        {
            isRunning = false;
        }
        else
        {
            isRunning = true;
        }
        
        anim.SetBool("isRunning", isRunning);

    }
    public void Walk()
    {
        if (myAgent.remainingDistance <= myAgent.stoppingDistance)
        {
         isMoving = false;
        }
        else
        {
         isMoving = true;
        }

        anim.SetBool("isMoving", isMoving);
    }
    private void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitinfo, 100, Ground))
            {
                myAgent.SetDestination(hitinfo.point);
            }
        }

        Run();
        if (Input.GetKey(KeyCode.N))
        {
          Walk();
        }
        else
        {
          Run();
        }
    }
}
