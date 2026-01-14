using UnityEngine;

public class TrashSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject trashPrefab;          // De prefab die gespawned wordt
    public Transform target;                // Object waar rondom gespawned wordt
    public float spawnRadius = 5f;          // Hoe ver rondom het object
    public float spawnInterval = 1f;        // Tijd tussen spawns

    private float timer = 0f;

    private void Update()
    {
        if (trashPrefab == null || target == null) return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnTrash();
            timer = 0f;
        }
    }

    void SpawnTrash()
    {
        // willekeurige positie in een cirkel rond het target
        Vector2 randomPos = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPos = new Vector3(
            target.position.x + randomPos.x,
            target.position.y,
            target.position.z + randomPos.y
        );

        Instantiate(trashPrefab, spawnPos, Quaternion.identity);
    }
}
