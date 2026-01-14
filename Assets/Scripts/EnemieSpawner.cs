using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;
    public float minSpawnTime = 10f;
    public float maxSpawnTime = 30f;

    private void Start()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            int spawnID = i; // index van spawnPoint = spawnID
            Transform sp = spawnPoints[i];
            StartCoroutine(SpawnLoop(sp, spawnID));
        }
    }

    private IEnumerator SpawnLoop(Transform spawnPoint, int spawnID)
    {
        while (true)
        {
            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(waitTime);

            SpawnEnemy(spawnPoint, spawnID);
        }
    }

    private void SpawnEnemy(Transform spawnPoint, int spawnID)
    {
        if (enemyPrefabs.Length == 0 || spawnPoint == null)
            return;

        Waypoint spawnWaypoint = spawnPoint.GetComponent<Waypoint>();
        if (spawnWaypoint == null)
        {
            Debug.LogError($"SpawnPoint {spawnPoint.name} mist Waypoint!");
            return;
        }

        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        if (!NavMesh.SamplePosition(spawnPoint.position, out NavMeshHit hit, 5f, NavMesh.AllAreas))
            return;

        GameObject enemy = Instantiate(prefab, hit.position, spawnPoint.rotation);
        EnemyAI ai = enemy.GetComponent<EnemyAI>();
        if (ai != null)
        {
            ai.Initialize(spawnWaypoint, spawnID); // spawnID meegeven
        }
    }
}
