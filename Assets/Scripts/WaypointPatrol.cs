using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaypointPatrol : MonoBehaviour
{
    public float chaseDistance = 10f;
    public Transform[] waypoints;
    public float movementSpeed = 3f;
    public Transform player;
    private int currentWaypointIndex = 0;
    private UnityEngine.AI.NavMeshAgent navAgent;

    void Start()
    {
        navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        navAgent.speed = movementSpeed;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        SetNextWaypoint();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (!navAgent.pathPending && navAgent.remainingDistance < 0.5f)
        {
            SetNextWaypoint();
        }
        else if(distanceToPlayer <= chaseDistance)
        {
            navAgent.SetDestination(player.position);
        }
    }

    void SetNextWaypoint()
    {
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        navAgent.SetDestination(waypoints[currentWaypointIndex].position);
    }
}
