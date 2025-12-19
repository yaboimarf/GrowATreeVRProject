using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [Header("Next Waypoints")]
    public Waypoint[] nextWaypoints;

    // Check of er volgende waypoints zijn
    public bool HasNext => nextWaypoints != null && nextWaypoints.Length > 0;

    // Kies random volgende waypoint
    public Waypoint GetRandomNextWaypoint()
    {
        if (!HasNext) return null;
        return nextWaypoints[Random.Range(0, nextWaypoints.Length)];
    }

    // Wanneer een NPC de waypoint raakt
    private void OnTriggerEnter(Collider other)
    {
        EnemyAI ai = other.GetComponent<EnemyAI>();
        if (ai != null)
        {
            ai.SetCurrentWaypoint(this);
        }
    }
}
