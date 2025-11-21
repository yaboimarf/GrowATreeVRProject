using UnityEditor.Searcher;
using UnityEngine;

public class TreeMiniGame : MonoBehaviour
{
    private float waterTimer;
    public float waterNeedTime;
    public bool waterNeed;
    public GameObject waterIndicator;
    private float waterCompletionTimer;
    public float waterCompletionTime;
    public bool waterMiniGameCompleted;

    private void Start()
    {
        waterTimer = waterNeedTime;
        waterCompletionTimer = waterCompletionTime;
    }
    private void Update()
    {
        if(waterNeed == true)
        {
            waterTimer -= Time.deltaTime;
            if(waterTimer <= 0)
            {
                WaterMiniGame();
            }
        }
        else
        {
            waterTimer = waterNeedTime;
            return;
        }

    }
    private void WaterMiniGame()
    {
        waterIndicator.SetActive(true);        
        waterCompletionTimer -= Time.deltaTime;
        if(waterMiniGameCompleted == false)
        {
            if (waterCompletionTimer <= 0)
            {
                waterIndicator.SetActive(false);
                waterTimer = waterNeedTime;
                waterCompletionTimer = waterCompletionTime;
                //subtract points
            }
        }
        else
        {
            waterIndicator.SetActive(false);
            waterTimer = waterNeedTime;
            waterCompletionTimer = waterCompletionTime;
            waterMiniGameCompleted = false;
            //add points
        }
    }    
}
