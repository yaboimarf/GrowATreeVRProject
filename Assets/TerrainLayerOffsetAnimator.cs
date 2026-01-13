using UnityEngine;

public class TerrainLayerOffsetAnimator : MonoBehaviour
{
    [Header("Instellingen")]
    [SerializeField] private Terrain terrain;              // Sleep je Terrain hiernaartoe (of auto-vinden)
    [SerializeField] private int layerIndex = 0;            // SPECIFIEKE LAYER: 0 = eerste layer (New Layer 1?), 1=volgende, etc.

    [Header("Animatie")]
    [SerializeField][Range(0f, 2f)] private float speedY = 0.1f;    // Snelheid omhoog (0.05 = langzaam, 0.2 = snel)
    [SerializeField][Range(-2f, 2f)] private float speedX = 0f;     // Optioneel: ook horizontaal bewegen

    private TerrainData terrainData;
    private TerrainLayer targetLayer;
    private Vector2 currentOffset;

    void Awake()
    {
        // Auto-vind Terrain als niet gezet
        if (terrain == null)
            terrain = GetComponent<Terrain>();

        if (terrain == null)
        {
            Debug.LogError("Geen Terrain gevonden! Zet hem in de Inspector.");
            return;
        }

        terrainData = terrain.terrainData;

        // Haal SPECIFIEKE LAYER op
        if (layerIndex >= 0 && layerIndex < terrainData.terrainLayers.Length)
        {
            targetLayer = terrainData.terrainLayers[layerIndex];
            currentOffset = targetLayer.tileOffset;
            Debug.Log($"Animating layer {layerIndex}: '{targetLayer.name}'");
        }
        else
        {
            Debug.LogError($"Layer index {layerIndex} bestaat niet! Er zijn {terrainData.terrainLayers.Length} layers.");
        }
    }

    void Update()
    {
        if (targetLayer == null) return;

        // Offset BEWEEGT AUTOMATISCH omhoog (Y) over tijd
        currentOffset.x += speedX * Time.deltaTime;
        currentOffset.y += speedY * Time.deltaTime;

        // Pas toe op SPECIFIEKE LAYER (terrain zelf BEWEegt NIET!)
        targetLayer.tileOffset = currentOffset;
    }
}