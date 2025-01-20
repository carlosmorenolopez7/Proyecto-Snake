using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    public Transform path;
    public float visionAngle = 60f;
    public float visionDistance = 10f;
    public Transform player;
    public float distanceThreshold = 2f;

    private NavMeshAgent agent;
    private Transform[] waypoints;
    private int childrenIndex = 0;
    private bool isChasingPlayer = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        waypoints = new Transform[path.childCount];
        for (int i = 0; i < path.childCount; i++)
        {
            waypoints[i] = path.GetChild(i);
        }

        SetDestinationToNextWaypoint();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
    }
}