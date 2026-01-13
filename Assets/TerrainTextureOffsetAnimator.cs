using UnityEngine;

public class TerrainTextureOffsetAnimator : MonoBehaviour
{
    [SerializeField] private Terrain terrain;           // sleep je Terrain component hierin
    [SerializeField] private int layerIndex = 0;        // welke terrain layer? (0 = eerste)

    [SerializeField] private float speedY = 0.05f;      // hoe snel wil je 'm omhoog laten bewegen
    [SerializeField] private float speedX = 0f;         // meestal alleen Y, maar kan ook X

    private TerrainData terrainData;
    private Vector2 currentOffset;

    void Awake()
    {
        if (terrain == null)
            terrain = GetComponent<Terrain>();

        terrainData = terrain.terrainData;
    }

    void Start()
    {
        // optioneel: start offset onthouden
        currentOffset = terrainData.terrainLayers[layerIndex].tileOffset;
    }

    void Update()
    {
        // elke frame iets hoger (of lager met negatieve speed)
        currentOffset.y += speedY * Time.deltaTime;
        currentOffset.x += speedX * Time.deltaTime;

        // pas toe op de layer
        TerrainLayer layer = terrainData.terrainLayers[layerIndex];
        layer.tileOffset = currentOffset;

        // belangrijk: forceer Unity om de verandering door te voeren
        terrainData.terrainLayers[layerIndex] = layer;
    }
}