using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class AgentPatrol : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    
    [SerializeField] private List<Transform> path;
    [SerializeField] private bool loopPath;
    [SerializeField] private float minTargetDistance = 0.5f;
    [SerializeField] private float chaseSpeedMult = 1.5f;
    [SerializeField] private float range = 20;

    [SerializeField] private GameObject taser;
    [SerializeField] private GiraffeBehaviour giraffe;
    
    public float SqrDisToGiraffe => (transform.position - giraffe.transform.position).sqrMagnitude;
    public bool Chasing => chaseGiraffe;
    private bool chaseGiraffe = false;
    
    private int targetIndex = 0;
    private int step = 1;

    private float defaultSpeed;
    private float defaultAcc;

    private void OnValidate()
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }
    }

    private void OnEnable()
    {
        giraffe.IsBad += ChaseGiraffe;
        giraffe.Tased += EndChase;
    }

    private void OnDisable()
    {
        giraffe.IsBad -= ChaseGiraffe;
        giraffe.Tased -= EndChase;
    }

    // Start is called before the first frame update
    void Start()
    {
        defaultSpeed = agent.speed;
        defaultAcc = agent.acceleration;
        CheckPath();
    }

    // Update is called once per frame
    void Update()
    {
        if (chaseGiraffe)
        {
            agent.SetDestination(giraffe.transform.position);
        }
        else
        {
            CheckPath();
        }
    }

    void ChaseGiraffe()
    {
        if ((transform.position - giraffe.transform.position).sqrMagnitude > range * range)
        {
            return;
        }
        
        chaseGiraffe = true;
        taser.SetActive(true);
        agent.speed = defaultSpeed * chaseSpeedMult;
        agent.acceleration = defaultAcc * chaseSpeedMult;
    }

    void EndChase()
    {
        chaseGiraffe = false;
        taser.SetActive(false);
        agent.speed = defaultSpeed;
        agent.acceleration = defaultAcc;

        agent.SetDestination(path[targetIndex].position);
    }

    private void CheckPath()
    {
        if (agent.remainingDistance <= minTargetDistance)
        {
            agent.SetDestination(path[targetIndex].position);

            if (!loopPath && (targetIndex >= path.Count - 1 || targetIndex <= 0))
            {
                step = targetIndex == 0 ? 1 : -1;
            }
            targetIndex = (targetIndex + step) % path.Count;
        }
    }
}
