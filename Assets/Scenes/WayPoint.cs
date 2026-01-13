using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Waypoint : MonoBehaviour
{
    [Header("Next waypoints per spawnpoint")]
    public Waypoint nextWaypoint1; // Voor enemies van spawnpoint 1
    public Waypoint nextWaypoint2; // Voor enemies van spawnpoint 2

    private void OnTriggerEnter(Collider other)
    {
        EnemyAI ai = other.GetComponent<EnemyAI>();
        if (ai != null)
        {
            ai.SetCurrentWaypoint(this);
        }
    }

    // Geeft de juiste next waypoint gebaseerd op spawnID
    public Waypoint GetNextWaypoint(int spawnID)
    {
        if (spawnID == 0)
            return nextWaypoint1;
        else if (spawnID == 1)
            return nextWaypoint2;

        return null; // fallback
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.25f);

        if (nextWaypoint1 != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, nextWaypoint1.transform.position);
        }

        if (nextWaypoint2 != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(transform.position, nextWaypoint2.transform.position);
        }
    }
#endif
}
