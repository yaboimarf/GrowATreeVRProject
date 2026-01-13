using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private int spawnID; // welke route deze enemy volgt

    private Waypoint currentWaypoint;

    public float moveSpeed = 2f;
    public float waitTimeAtWaypoint = 0.5f;
    private bool waiting;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
    }

    // Initialize enemy met spawnID
    public void Initialize(Waypoint spawnWaypoint, int spawnID)
    {
        this.spawnID = spawnID;

        if (spawnWaypoint == null)
        {
            Debug.LogError($"{name} kreeg geen spawnWaypoint");
            return;
        }

        currentWaypoint = spawnWaypoint.GetNextWaypoint(spawnID);
        if (currentWaypoint != null)
        {
            agent.SetDestination(currentWaypoint.transform.position);
        }
    }

    private void Update()
    {
        // niks nodig; movement via waypoint trigger
    }

    public void SetCurrentWaypoint(Waypoint waypoint)
    {
        if (waypoint == null)
            return;

        currentWaypoint = waypoint.GetNextWaypoint(spawnID);
        if (currentWaypoint != null)
        {
            agent.SetDestination(currentWaypoint.transform.position);
        }
    }
}
