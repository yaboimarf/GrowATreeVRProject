using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject[] enemyPrefabs; // <<< meerdere prefabs
    public int amountToSpawn = 5;
    public float spawnRadius = 20f;

    [Header("Spawn Point")]
    public Transform spawnPoint;

    [Header("Safety Settings")]
    public float minDistanceFromPlayer = 5f;

    private Transform player;

    private void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
            player = p.transform;
        else
            Debug.LogWarning("Player object met tag 'Player' niet gevonden!");

        for (int i = 0; i < amountToSpawn; i++)
        {
            Vector3 spawnPos = GetSafeSpawnPosition();

            NavMeshHit hit;
            if (NavMesh.SamplePosition(spawnPos, out hit, 2f, NavMesh.AllAreas))
            {
                spawnPos = hit.position;

                GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
                Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("Kon geen NavMesh positie vinden voor enemy spawn!");
            }
        }
    }

    private Vector3 GetSafeSpawnPosition()
    {
        Vector3 basePosition = (spawnPoint != null) ? spawnPoint.position : transform.position;
        Vector3 spawnPos;
        int attempts = 0;

        do
        {
            spawnPos = basePosition + Random.insideUnitSphere * spawnRadius;
            spawnPos.y = 0;

            attempts++;
            if (attempts > 40)
                break;

        } while (player != null && Vector3.Distance(spawnPos, player.position) < minDistanceFromPlayer);

        return spawnPos;
    }
}
