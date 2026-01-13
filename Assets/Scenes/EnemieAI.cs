using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;
    public float moveSpeed = 2f;
    public float waitTimeAtWaypoint = 0.5f;

    private Waypoint currentWaypoint;
    private bool waiting;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
    }

    public void Initialize(Waypoint spawnWaypoint)
    {
        if (spawnWaypoint == null)
        {
            Debug.LogError($"{name} kreeg geen spawnWaypoint");
            return;
        }

        if (!spawnWaypoint.HasNext)
        {
            Debug.LogError($"{spawnWaypoint.name} heeft geen nextWaypoints");
            return;
        }

        currentWaypoint = spawnWaypoint.GetRandomNextWaypoint();
        agent.SetDestination(currentWaypoint.transform.position);
    }

    private void Update()
    {
        // Nu hoeven we hier niks te checken voor remainingDistance
    }

    // Wordt aangeroepen door Waypoint trigger
    public void SetCurrentWaypoint(Waypoint waypoint)
    {
        if (waypoint == null)
            return;

        currentWaypoint = waypoint.GetRandomNextWaypoint();
        if (currentWaypoint != null)
        {
            agent.SetDestination(currentWaypoint.transform.position);
        }
    }
}
