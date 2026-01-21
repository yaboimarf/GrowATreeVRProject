using UnityEngine;

public class BeetleSpawner : MonoBehaviour
{
    [Header("Spawner Toggle")]
    public bool spawnBeetles = true;   // 👈 AAN/UIT in Inspector

    [Header("Beetle Settings")]
    public GameObject[] beetlePrefabs;    // Meerdere kever prefabs
    public float spawnRadius = 3f;        // Radius rondom de spawner waar kevers verschijnen
    public float minSpawnTime = 1f;       // Minimum tijd tussen spawns
    public float maxSpawnTime = 5f;       // Maximum tijd tussen spawns

    private float timer;

    private void Start()
    {
        ResetTimer();
    }

    private void Update()
    {
        // ❌ Als spawner uit staat → niks doen
        if (!spawnBeetles)
            return;

        if (beetlePrefabs == null || beetlePrefabs.Length == 0)
            return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            SpawnBeetle();
            ResetTimer();
        }
    }

    private void ResetTimer()
    {
        timer = Random.Range(minSpawnTime, maxSpawnTime);
    }

    private void SpawnBeetle()
    {
        // Kies random prefab
        GameObject prefab = beetlePrefabs[Random.Range(0, beetlePrefabs.Length)];

        // Willekeurige positie in XZ vlak binnen spawnRadius
        Vector2 randomPos = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPos = new Vector3(
            transform.position.x + randomPos.x,  // X
            transform.position.y,                // Y
            transform.position.z + randomPos.y   // Z
        );

        // Willekeurige rotatie rondom Y-as
        Quaternion randomRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

        Instantiate(prefab, spawnPos, randomRotation);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
#endif
}
