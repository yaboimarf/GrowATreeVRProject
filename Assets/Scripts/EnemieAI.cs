using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private int spawnID;                 // Welke route deze enemy volgt
    private Waypoint currentWaypoint;

    [Header("Movement")]
    public float moveSpeed = 2f;
    public float waitTimeAtWaypoint = 0.5f;

    private bool isWaiting;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        agent.autoBraking = true;

        // Zorg dat deze NPC de juiste tag heeft
        if (gameObject.tag != "Enemy")
            gameObject.tag = "Enemy";
    }

    /// <summary>
    /// Wordt aangeroepen door de spawner
    /// </summary>
    public void Initialize(Waypoint spawnWaypoint, int spawnID)
    {
        this.spawnID = spawnID;

        if (spawnWaypoint == null)
        {
            Debug.LogError($"{name} kreeg geen spawnWaypoint");
            Despawn();
            return;
        }

        Waypoint next = spawnWaypoint.GetNextWaypoint(spawnID);

        if (next == null)
        {
            // Geen route voor deze spawnID → despawn
            Despawn();
            return;
        }

        currentWaypoint = next;
        agent.SetDestination(currentWaypoint.transform.position);
    }

    /// <summary>
    /// Wordt aangeroepen door Waypoint.OnTriggerEnter
    /// </summary>
    public void SetCurrentWaypoint(Waypoint waypoint)
    {
        if (isWaiting || waypoint == null)
            return;

        StartCoroutine(HandleWaypointReached(waypoint));
    }

    private System.Collections.IEnumerator HandleWaypointReached(Waypoint waypoint)
    {
        isWaiting = true;

        if (waitTimeAtWaypoint > 0f)
            yield return new WaitForSeconds(waitTimeAtWaypoint);

        Waypoint next = waypoint.GetNextWaypoint(spawnID);

        if (next == null)
        {
            // Laatste waypoint bereikt
            Despawn();
            yield break;
        }

        currentWaypoint = next;
        agent.SetDestination(currentWaypoint.transform.position);
        isWaiting = false;
    }

    private void Despawn()
    {
        Destroy(gameObject);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (currentWaypoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, currentWaypoint.transform.position);
        }
    }
#endif
}
