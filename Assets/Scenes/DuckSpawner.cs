using UnityEngine;
using UnityEngine.AI;

public class DuckSpawner : MonoBehaviour
{
    [Header("Enemy Settings")]
    public GameObject[] enemyPrefabs;

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    [Header("Spawn Amount")]
    public int minDucksPerSpawn = 10;
    public int maxDucksPerSpawn = 25;

    [Header("Spawn Radius")]
    public float spawnRadius = 5f; // radius rond spawnpoint

    [Header("Player Detection")]
    public float detectRadius = 3f; // detectieradius van de eenden

    [Header("Duck Scale")]
    public float minScale = 0.8f;  // minimale schaal
    public float maxScale = 1.3f;  // maximale schaal

    private void Start()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            SpawnEnemies(spawnPoint);
        }
    }

    private void SpawnEnemies(Transform spawnPoint)
    {
        if (enemyPrefabs.Length == 0 || spawnPoint == null) return;

        int spawnCount = Random.Range(minDucksPerSpawn, maxDucksPerSpawn + 1);

        // Zoek speler automatisch op tag "Player"
        GameObject playerObj = GameObject.FindGameObjectWithTag("MainCamera");
        if (playerObj == null)
        {
            Debug.LogWarning("Player object with tag 'Player' not found in scene.");
        }

        for (int i = 0; i < spawnCount; i++)
        {
            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

            // Kies een random positie in een cirkel rond spawnpoint
            Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
            Vector3 tentativePosition = spawnPoint.position + new Vector3(randomCircle.x, 0f, randomCircle.y);

            // Correctie op NavMesh zodat eenden op de grond spawnen
            if (NavMesh.SamplePosition(tentativePosition, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                GameObject duck = Instantiate(enemyPrefab, hit.position, Quaternion.identity);

                // Stel een random scale in
                float randomScale = Random.Range(minScale, maxScale);
                duck.transform.localScale = Vector3.one * randomScale;

                DuckAI duckAI = duck.GetComponent<DuckAI>();
                if (duckAI != null)
                {
                    // Geef spawn center en radius door
                    duckAI.SetPatrolCenterAndRadius(spawnPoint.position, spawnRadius);

                    // Stel detectRadius direct in via public variabele
                    duckAI.detectRadius = detectRadius;

                    // Koppel speler
                    if (playerObj != null)
                        duckAI.SetPlayer(playerObj.transform);
                }
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (spawnPoints == null) return;

        Gizmos.color = Color.red;
        foreach (Transform sp in spawnPoints)
        {
            if (sp != null)
            {
                Gizmos.DrawSphere(sp.position, 0.4f);

                // Toon spawn radius als cirkel
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(sp.position, spawnRadius);
                Gizmos.color = Color.red;
            }
        }
    }
#endif
}
