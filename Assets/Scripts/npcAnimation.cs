using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class npcAnimation : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private AgentPatrol patrol;

    [SerializeField] private float taseDistance = 1;
    

    private void OnValidate()
    {
        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }

        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        if (patrol == null)
        {
            patrol = GetComponent<AgentPatrol>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        anim.SetFloat("Speed", agent.velocity.magnitude);

        if (patrol != null)
        {
            anim.SetBool("Taze", 
                patrol.Chasing && patrol.SqrDisToGiraffe <= taseDistance * taseDistance);
        }
    }
}
