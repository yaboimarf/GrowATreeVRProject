using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;

    [Header("Path Settings")]
    public string waypointTag = "WayPoint"; // tag van alle waypoints in park
    public float waypointReachDistance = 0.5f;
    public float moveSpeed = 2f;
    public float waitTimeAtWaypoint = 1.5f;

    private Waypoint[] allWaypoints;
    private Waypoint currentWaypoint;
    private float waitTimer;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = true;
        agent.angularSpeed = 720f;
        agent.acceleration = 8f;
    }

    private void Start()
    {
        // Zoek alle waypoints via tag
        GameObject[] waypointObjects = GameObject.FindGameObjectsWithTag(waypointTag);
        allWaypoints = new Waypoint[waypointObjects.Length];
        for (int i = 0; i < waypointObjects.Length; i++)
        {
            allWaypoints[i] = waypointObjects[i].GetComponent<Waypoint>();
        }

        // Kies dichtstbijzijnde waypoint van spawn
        currentWaypoint = GetClosestWaypoint();
        agent.speed = moveSpeed;

        if (currentWaypoint != null)
            agent.SetDestination(currentWaypoint.transform.position);
    }

    private void Update()
    {
        if (!agent.isOnNavMesh || currentWaypoint == null) return;

        float distance = Vector3.Distance(transform.position, currentWaypoint.transform.position);

        if (distance <= waypointReachDistance)
        {
            waitTimer += Time.deltaTime;
            agent.isStopped = true;

            if (waitTimer >= waitTimeAtWaypoint)
            {
                ChooseNextWaypoint();
                waitTimer = 0f;
            }
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(currentWaypoint.transform.position);
        }
    }

    private void ChooseNextWaypoint()
    {
        if (currentWaypoint.HasNext)
        {
            currentWaypoint = GetClosestNextWaypoint(currentWaypoint.nextWaypoints);
            agent.SetDestination(currentWaypoint.transform.position);
        }
        else
        {
            // Eind van pad
            agent.isStopped = true;
        }
    }

    // Kies de dichtstbijzijnde waypoint uit een lijst van opties
    private Waypoint GetClosestNextWaypoint(Waypoint[] options)
    {
        if (options == null || options.Length == 0) return null;

        Waypoint closest = options[0];
        float minDist = Vector3.Distance(transform.position, closest.transform.position);

        foreach (var wp in options)
        {
            float dist = Vector3.Distance(transform.position, wp.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = wp;
            }
        }

        return closest;
    }

    private Waypoint GetClosestWaypoint()
    {
        if (allWaypoints == null || allWaypoints.Length == 0) return null;

        Waypoint closest = allWaypoints[0];
        float minDist = Vector3.Distance(transform.position, closest.transform.position);

        foreach (var wp in allWaypoints)
        {
            float dist = Vector3.Distance(transform.position, wp.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = wp;
            }
        }

        return closest;
    }
}
