using UnityEditor.Searcher;
using UnityEngine;

public class TreeMiniGame : MonoBehaviour
{
    private float waterTimer;
    public float waterNeedTime;
    public bool waterNeed;
    private float manureTimer;
    private float manureNeedTime;
    public bool manureNeed;
    private float insectSpawnTimer;
    private float insectSpawnTime;
    public bool insectSpawn;
    public bool trashSpawn;
    private void Start()
    {
        
    }

    private void Update()
    {
        if(waterNeed == true)
        {
            waterTimer -= Time.deltaTime;
            if(waterTimer <= 0)
            {
                waterTimer += waterNeedTime;
            }
        }
        if(manureNeed == true)
        {
            manureTimer -= Time.deltaTime;
            if(manureTimer <= 0)
            {
                manureTimer += manureNeedTime;
            }
        }
        if(insectSpawn == true)
        {
            insectSpawnTimer -= Time.deltaTime;
            if(insectSpawnTimer <= 0)
            {
                insectSpawnTimer += insectSpawnTime;
            }
        }
        if(trashSpawn == true)
        {

        }
    }
}
