using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject[] enemyPrefabs;
    public int amountToSpawn = 5;
    public float spawnRadius = 20f;

    [Header("Spawn Point")]
    public Transform spawnPoint;

    [Header("Safety Settings")]
    public float minDistanceFromPlayer = 5f;

    [Header("Vertical Offset")]
    public float verticalOffset = 0.5f; // klein beetje omhoog/omlaag

    private Transform player;

    private void Start()
    {
        // Player vinden (pas aan indien nodig)
        GameObject p = GameObject.FindGameObjectWithTag("MainCamera");
        if (p != null)
            player = p.transform;
        else
            Debug.LogWarning("Player object met tag 'Player' niet gevonden!");

        for (int i = 0; i < amountToSpawn; i++)
        {
            Vector3 spawnPos = GetSafeSpawnPosition();

            // Zorg dat spawn op NavMesh ligt
            if (NavMesh.SamplePosition(spawnPos, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
                Instantiate(enemyPrefab, hit.position, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("Kon geen NavMesh positie vinden voor enemy spawn!");
            }
        }
    }

    private Vector3 GetSafeSpawnPosition()
    {
        Vector3 basePosition = spawnPoint != null ? spawnPoint.position : transform.position;
        Vector3 spawnPos = basePosition;
        int attempts = 0;

        do
        {
            // Alleen X/Z richting (links/rechts/voor/achter + diagonaal)
            Vector2 random2D = Random.insideUnitCircle * spawnRadius;

            spawnPos = new Vector3(
                basePosition.x + random2D.x,
                basePosition.y + Random.Range(-verticalOffset, verticalOffset),
                basePosition.z + random2D.y
            );

            attempts++;
            if (attempts >= 40)
                break;

        } while (player != null &&
                 Vector3.Distance(spawnPos, player.position) < minDistanceFromPlayer);

        return spawnPos;
    }

#if UNITY_EDITOR
    // Debug visualisatie in Scene view als platte cirkel (2D disc)
    private void OnDrawGizmosSelected()
    {
        Vector3 basePosition = spawnPoint != null ? spawnPoint.position : transform.position;

        // Platte cirkel op X/Z vlak
        Gizmos.color = Color.red;
        int segments = 40; // hoe glad de cirkel is
        float angleStep = 360f / segments;
        Vector3 prevPoint = basePosition + new Vector3(spawnRadius, 0, 0);

        for (int i = 1; i <= segments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 nextPoint = basePosition + new Vector3(Mathf.Cos(angle) * spawnRadius, 0, Mathf.Sin(angle) * spawnRadius);
            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;
        }

        // Optioneel: veilige afstand van speler
        if (player != null)
        {
            Gizmos.color = Color.yellow;
            float playerRadius = minDistanceFromPlayer;
            Vector3 pPrev = player.position + new Vector3(playerRadius, 0, 0);
            for (int i = 1; i <= segments; i++)
            {
                float angle = i * angleStep * Mathf.Deg2Rad;
                Vector3 pNext = player.position + new Vector3(Mathf.Cos(angle) * playerRadius, 0, Mathf.Sin(angle) * playerRadius);
                Gizmos.DrawLine(pPrev, pNext);
                pPrev = pNext;
            }
        }
    }
#endif
}
