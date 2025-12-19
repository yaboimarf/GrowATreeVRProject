using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;

    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float waitTimeAtWaypoint = 0.5f; // optioneel: kleine pauze bij waypoint

    [Header("Spawn Waypoint")]
    public Waypoint spawnWaypoint;

    private Waypoint currentWaypoint;
    private bool waiting;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
    }

    private void Start()
    {
        if (spawnWaypoint != null && spawnWaypoint.HasNext)
        {
            // Start direct naar de eerste next waypoint van spawn
            currentWaypoint = spawnWaypoint.GetRandomNextWaypoint();
            agent.SetDestination(currentWaypoint.transform.position);
        }
    }

    private void Update()
    {
        if (currentWaypoint != null && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!waiting)
            {
                waiting = true;
                Invoke(nameof(MoveToNextWaypoint), waitTimeAtWaypoint);
            }
        }
    }

    // Wordt aangeroepen door Waypoint trigger of na waitTime
    public void SetCurrentWaypoint(Waypoint waypoint)
    {
        currentWaypoint = waypoint;
        MoveToNextWaypoint();
    }

    private void MoveToNextWaypoint()
    {
        waiting = false;

        if (currentWaypoint != null && currentWaypoint.HasNext)
        {
            Waypoint next = currentWaypoint.GetRandomNextWaypoint();
            if (next != null)
            {
                currentWaypoint = next;
                agent.SetDestination(currentWaypoint.transform.position);
            }
        }
    }
}
