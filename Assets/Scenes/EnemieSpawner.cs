using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Settings")]
    public GameObject[] enemyPrefabs;

    [Header("Spawn Points")]
    public Transform[] spawnPoints; // zoveel spawnpoints als je wilt

    [Header("Spawn Rate Per SpawnPoint")]
    public float minSpawnTime = 10f;
    public float maxSpawnTime = 30f;

    private void Start()
    {
        // Start voor elke spawnpoint een eigen spawn-loop
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

        // Zorg dat hij op de NavMesh spawn
        if (NavMesh.SamplePosition(spawnPoint.position, out NavMeshHit hit, 2f, NavMesh.AllAreas))
        {
            Instantiate(enemyPrefab, hit.position, spawnPoint.rotation);
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
