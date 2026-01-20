using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class TrashSpawner : MonoBehaviour
{
    [Header("Trash Settings")]
    public GameObject[] trashPrefabs;  // Meerdere trash prefabs
    public float spawnRadius = 3f;     // Radius rondom de tree waar trash wordt gespawned
    public string npcTag = "Enemy";    // Tag van NPCs

    [Header("Trigger Settings")]
    public float triggerRadius = 5f;   // Radius die NPCs detecteert

    private SphereCollider triggerCollider;

    private void Awake()
    {
        // Zorg dat de collider aanwezig is en juiste trigger radius heeft
        triggerCollider = GetComponent<SphereCollider>();
        triggerCollider.isTrigger = true;
        triggerCollider.radius = triggerRadius;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Alleen NPCs met de juiste tag triggeren
        if (!other.CompareTag(npcTag))
            return;

        SpawnTrash();
    }

    private void SpawnTrash()
    {
        if (trashPrefabs == null || trashPrefabs.Length == 0)
            return;

        // Kies random prefab
        GameObject prefab = trashPrefabs[Random.Range(0, trashPrefabs.Length)];

        // Willekeurige positie in XZ vlak binnen spawnRadius
        Vector2 randomPos = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPos = new Vector3(
            transform.position.x + randomPos.x,  // X
            transform.position.y,                // Y = tree hoogte
            transform.position.z + randomPos.y   // Z
        );

        Instantiate(prefab, spawnPos, Quaternion.identity);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        // Spawn radius (groen)
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);

        // Trigger radius (geel)
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, triggerRadius);
    }
#endif
}
