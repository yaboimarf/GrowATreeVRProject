using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Settings")]
    public GameObject[] enemyPrefabs;

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    [Header("Spawn Rate Per SpawnPoint")]
    public float minSpawnTime = 10f;
    public float maxSpawnTime = 30f;

    private void Start()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            if (spawnPoint != null)
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

        Waypoint spawnWaypoint = spawnPoint.GetComponent<Waypoint>();
        if (spawnWaypoint == null)
        {
            Debug.LogError($"SpawnPoint {spawnPoint.name} mist Waypoint component!");
            return;
        }

        GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        // Zoek een geldige NavMesh positie
        if (!NavMesh.SamplePosition(spawnPoint.position, out NavMeshHit hit, 5f, NavMesh.AllAreas))
        {
            Debug.LogError($"Geen NavMesh gevonden bij spawnpoint {spawnPoint.name}");
            return;
        }

        GameObject enemy = Instantiate(enemyPrefab, hit.position, spawnPoint.rotation);

        EnemyAI ai = enemy.GetComponent<EnemyAI>();
        if (ai != null)
        {
            ai.Initialize(spawnWaypoint); // Dit moet nu werken
        }
        else
        {
            Debug.LogError("Enemy prefab mist EnemyAI script!");
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
                Gizmos.DrawSphere(sp.position, 0.4f);
        }
    }
#endif
}
