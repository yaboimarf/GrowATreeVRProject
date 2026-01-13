using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Waypoint : MonoBehaviour
{
    public Waypoint[] nextWaypoints;

    public bool HasNext => nextWaypoints != null && nextWaypoints.Length > 0;

    public Waypoint GetRandomNextWaypoint()
    {
        if (!HasNext)
            return null;

        return nextWaypoints[Random.Range(0, nextWaypoints.Length)];
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyAI ai = other.GetComponent<EnemyAI>();
        if (ai != null)
        {
            ai.SetCurrentWaypoint(this);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.25f);

        if (HasNext)
        {
            Gizmos.color = Color.cyan;
            foreach (Waypoint wp in nextWaypoints)
            {
                if (wp != null)
                    Gizmos.DrawLine(transform.position, wp.transform.position);
            }
        }
    }
#endif
}
