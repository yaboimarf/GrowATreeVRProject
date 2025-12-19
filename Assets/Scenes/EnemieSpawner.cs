using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Settings")]
    public GameObject[] enemyPrefabs;

    [Header("Spawn Points")]
    public Transform[] spawnPoints; // Spawnpoints in de scene

    [Header("Spawn Rate Per SpawnPoint")]
    public float minSpawnTime = 10f;
    public float maxSpawnTime = 30f;

    private void Start()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            StartCoroutine(SpawnLoop(spawnPoint));
        }
    }

    private IEnumerator SpawnLoop(Transform spawnPoint)
    {
        while (true)
        {
            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(waitTime);

            SpawnEnemy(spawnPoint);
        }
    }

    private void SpawnEnemy(Transform spawnPoint)
    {
        if (enemyPrefabs.Length == 0 || spawnPoint == null)
            return;

        GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        // Spawn op NavMesh
        if (NavMesh.SamplePosition(spawnPoint.position, out NavMeshHit hit, 2f, NavMesh.AllAreas))
        {
            GameObject enemy = Instantiate(enemyPrefab, hit.position, spawnPoint.rotation);

            // Koppel spawnWaypoint automatisch
            Waypoint spawnWaypoint = spawnPoint.GetComponent<Waypoint>();
            if (spawnWaypoint != null)
            {
                EnemyAI ai = enemy.GetComponent<EnemyAI>();
                if (ai != null)
                {
                    ai.spawnWaypoint = spawnWaypoint;
                }
            }
            else
            {
                Debug.LogWarning($"SpawnPoint {spawnPoint.name} heeft geen Waypoint component!");
            }
        }
        else
        {
            Debug.LogWarning($"Geen NavMesh gevonden bij spawnpoint: {spawnPoint.name}");
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
            }
        }
    }
#endif
}
