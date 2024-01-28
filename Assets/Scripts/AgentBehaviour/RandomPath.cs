using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class RandomPath : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private NavMeshObstacle ob;

    [SerializeField] private Transform maxBounds;
    [SerializeField] private Transform minBounds;

    [SerializeField] private float minWalkDistance = 1;
    [SerializeField] private float maxWalkDistance = 10;
    [SerializeField] private float minStopTime = 1;
    [SerializeField] private float maxStopTime = 5;
    [SerializeField] private float targetDistance = 0.25f;
    
    [SerializeField] private List<Transform> POI;
    [SerializeField] private float POIchance = 0.1f;

    [SerializeField] private GiraffeBehaviour giraffe;

    [SerializeField] private float runMult = 2;
    [SerializeField] private float scaredDistance;
    [SerializeField] private float scaredTime = 5;

    private float defaultSpeed;
    private float defaultAcc;
    
    private float stopTimestamp = 0;
    private float currentStopTime = -1;

    private float scaredStart = -5;
    

    private void OnValidate()
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }
        if (ob == null)
        {
            ob = GetComponent<NavMeshObstacle>();
        }
    }

    private void OnEnable()
    {
        giraffe.IsBad += RunAway;
    }

    private void OnDisable()
    {
        giraffe.IsBad -= RunAway;
    }

    // Start is called before the first frame update
    void Start()
    {
        defaultSpeed = agent.speed;
        defaultAcc = agent.acceleration;
        agent.SetDestination(GetNewTarget());
    }
    

    // Update is called once per frame
    void Update()
    {
        if (Time.time - scaredStart > scaredTime)
        {
            if (currentStopTime > 0 && Time.time - stopTimestamp > currentStopTime)
            {
                agent.enabled = true;
                ob.enabled = false;
                agent.SetDestination(GetNewTarget());
                currentStopTime = -1;
                return;
            }

            if (currentStopTime < 0 && agent.remainingDistance < targetDistance)
            {
                stopTimestamp = Time.time;
                currentStopTime = Random.Range(minStopTime, maxStopTime);

                agent.enabled = false;
                ob.enabled = true;

                if (agent.speed > defaultSpeed)
                {
                    agent.speed = defaultSpeed;
                    agent.acceleration = defaultAcc;
                }
            }
        }
    }

    private void RunAway()
    {
        if ((transform.position - giraffe.transform.position).sqrMagnitude < scaredDistance * scaredDistance)
        {
            //Debug.Log("help");
            agent.speed = defaultSpeed * runMult;
            agent.acceleration = defaultAcc * runMult;
            
            agent.enabled = true;
            ob.enabled = false;
            currentStopTime = -1;

            scaredStart = Time.time;
            
            agent.SetDestination(GetRunPos());
        }
    }

    private Vector3 GetRunPos()
    {
        var dir = (transform.position - giraffe.transform.position).normalized;
        return ClampPosition(transform.position + dir * scaredDistance * 1.2f);
    }

    private Vector3 GetNewTarget()
    {
        if (Random.value < POIchance && POI.Count > 0)
        {
            return POI[Random.Range(0, POI.Count)].position;
        }
        
        float angle = Random.Range(0, 2 * Mathf.PI);
        Vector3 dPos = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));
        dPos *= Random.Range(minWalkDistance, maxWalkDistance);

        return ClampPosition(transform.position + dPos);
    }

    private Vector3 ClampPosition(Vector3 pos)
    {
        pos.x = Mathf.Clamp(pos.x, minBounds.position.x, maxBounds.position.x);
        pos.z = Mathf.Clamp(pos.z, minBounds.position.z, maxBounds.position.z);
        return pos;
    }

    private void OnDrawGizmos()
    {
        if (maxBounds == null || minBounds == null) { return;}
        
        float y = (minBounds.position.y + maxBounds.position.y) / 2;

        Vector3 c1 = new Vector3(minBounds.position.x, y, minBounds.position.z);
        Vector3 c2 = new Vector3(maxBounds.position.x, y, minBounds.position.z);
        Vector3 c3 = new Vector3(maxBounds.position.x, y, maxBounds.position.z);
        Vector3 c4 = new Vector3(minBounds.position.x, y, maxBounds.position.z);

        if (minBounds.position.x > maxBounds.position.x || minBounds.position.z > maxBounds.position.z)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.green;
        }
        Gizmos.DrawLine(c1, c2);
        Gizmos.DrawLine(c2, c3);
        Gizmos.DrawLine(c3, c4);
        Gizmos.DrawLine(c4, c1);
    }
}
