using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentPatrol : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    
    [SerializeField] private List<Transform> path;
    [SerializeField] private bool loopPath;
    [SerializeField] private float minTragetDistance = 0.5f;

    private int targetIndex = 0;
    private int step = 1;
    
    private void OnValidate()
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CheckPath();
    }

    // Update is called once per frame
    void Update()
    {
        CheckPath();
    }

    private void CheckPath()
    {
        if (agent.remainingDistance <= minTragetDistance)
        {
            agent.SetDestination(path[targetIndex].position);

            if (!loopPath && (targetIndex >= path.Count - 1 || targetIndex <= 0))
            {
                step *= -1;
            }
            targetIndex = (targetIndex + step) % path.Count;
        }
    }
}
