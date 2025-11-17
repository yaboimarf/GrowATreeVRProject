using UnityEditor.Searcher;
using UnityEngine;

public class TreeMiniGame : MonoBehaviour
{
    public float waterTimer;
    public float waterNeedTime;
    public bool waterNeed;
    public float manureTimer;
    public float manureNeedTime;
    public bool manureNeed;
    public float insectSpawnTimer;
    public float insectSpawnTime;
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
