using UnityEngine;

[System.Serializable]
public class Waypoint : MonoBehaviour
{
    [Header("Waypoint Settings")]
    public Waypoint[] nextWaypoints; // alle mogelijke volgende waypoints

    // Hulpfunctie: check of waypoint een afslag heeft
    public bool HasNext => nextWaypoints != null && nextWaypoints.Length > 0;

    // Kies willekeurig een volgende waypoint
    public Waypoint GetRandomNextWaypoint()
    {
        if (!HasNext) return null;
        return nextWaypoints[Random.Range(0, nextWaypoints.Length)];
    }
}
