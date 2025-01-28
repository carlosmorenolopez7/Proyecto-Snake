using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    public Transform path;
    public float visionAngle = 45f;
    public float visionDistance = 10f;
    public Transform player;
    public float distanceThreshold = 2f;
    public float searchSpeedMultiplier = 2.5f;
    public float searchRadius = 20f;
    public float waypointWaitTime = 3f;
    public float minDistanceFromWall = 5f;

    private NavMeshAgent agent;
    private Transform[] waypoints;
    private int childrenIndex = 0;
    private bool isChasingPlayer = false;
    private bool isSearchingPlayer = false;
    private Vector3[] searchWaypoints;
    private bool speedIncreased = false;

    /*void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        waypoints = new Transform[path.childCount];
        for (int i = 0; i < path.childCount; i++)
        {
            waypoints[i] = path.GetChild(i);
        }

        SetDestinationToNextWaypoint();
    }

    void Update()
    {
        if (isChasingPlayer)
        {
            ChasePlayer();
        }
        else
        {
            MoveAlongPath();
            DetectPlayer();
        }
    }

    private void MoveAlongPath()
    {
        if (Vector3.Distance(transform.position, waypoints[childrenIndex].position) < distanceThreshold)
        {
            childrenIndex = (childrenIndex + 1) % waypoints.Length;
            SetDestinationToNextWaypoint();
        }
    }

    private void SetDestinationToNextWaypoint()
    {
        agent.SetDestination(waypoints[childrenIndex].position);
    }

    private void DetectPlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float angle = Vector3.Angle(directionToPlayer, transform.forward);

        if (angle < visionAngle / 2f && Vector3.Distance(transform.position, player.position) < visionDistance)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, directionToPlayer.normalized, out hit, visionDistance))
            {
                if (hit.transform == player)
                {
                    isChasingPlayer = true;
                }
            }
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        Vector3 directionToPlayer = player.position - transform.position;
        float angle = Vector3.Angle(directionToPlayer, transform.forward);

        if (angle > visionAngle / 2f || Vector3.Distance(transform.position, player.position) > visionDistance)
        {
            isChasingPlayer = false;
            SetDestinationToNextWaypoint();
        }
    }*/

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        waypoints = new Transform[path.childCount];
        for (int i = 0; i < path.childCount; i++)
        {
            waypoints[i] = path.GetChild(i);
        }

        SetDestinationToNextWaypoint();
        StartCoroutine(Patrol());
    }

    private IEnumerator Patrol()
    {
        while (true)
        {
            if (!isChasingPlayer && !isSearchingPlayer)
            {
                MoveAlongPath();
                DetectPlayer();
            }
            yield return null;
        }
    }

    private void MoveAlongPath()
    {
        if (Vector3.Distance(transform.position, waypoints[childrenIndex].position) < distanceThreshold)
        {
            childrenIndex = (childrenIndex + 1) % waypoints.Length;
            SetDestinationToNextWaypoint();
        }
    }

    private void SetDestinationToNextWaypoint()
    {
        agent.SetDestination(waypoints[childrenIndex].position);
    }

    private void DetectPlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float angle = Vector3.Angle(directionToPlayer, transform.forward);

        if (angle < visionAngle / 2f && Vector3.Distance(transform.position, player.position) < visionDistance)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, directionToPlayer.normalized, out hit, visionDistance))
            {
                if (hit.transform == player)
                {
                    isChasingPlayer = true;
                    StartCoroutine(ChasePlayer());
                }
            }
        }
    }

    private IEnumerator ChasePlayer()
    {
        while (isChasingPlayer)
        {
            agent.SetDestination(player.position);
            Vector3 directionToPlayer = player.position - transform.position;
            float angle = Vector3.Angle(directionToPlayer, transform.forward);

            if (angle > visionAngle / 2f || Vector3.Distance(transform.position, player.position) > visionDistance)
            {
                isChasingPlayer = false;
                isSearchingPlayer = true;
                StartCoroutine(SearchPlayer());
            }
            yield return null;
        }
    }

    private IEnumerator SearchPlayer()
    {
        if (!speedIncreased)
        {
            agent.speed *= searchSpeedMultiplier;
            speedIncreased = true;
        }

        yield return new WaitForSeconds(waypointWaitTime);
        searchWaypoints = new Vector3[5];
        for (int i = 0; i < searchWaypoints.Length; i++)
        {
            Vector3 randomDirection;
            NavMeshHit hit;
            do
            {
                randomDirection = Random.insideUnitSphere * searchRadius;
                randomDirection += player.position;
                NavMesh.SamplePosition(randomDirection, out hit, searchRadius, 1);
            } while (Vector3.Distance(hit.position, player.position) < minDistanceFromWall);

            searchWaypoints[i] = hit.position;
        }

        int waypointIndex = 0;
        while (isSearchingPlayer)
        {
            agent.SetDestination(searchWaypoints[waypointIndex]);
            while (Vector3.Distance(transform.position, searchWaypoints[waypointIndex]) > distanceThreshold)
            {
                DetectPlayer();
                yield return null;
            }

            waypointIndex = (waypointIndex + 1) % searchWaypoints.Length;
        }

        isSearchingPlayer = false;
        StartCoroutine(Patrol());
    }
}