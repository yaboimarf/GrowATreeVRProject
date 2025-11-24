using UnityEditor.Searcher;
using UnityEngine;
using UnityEngine.UI;

public class TreeMiniGame : MonoBehaviour
{
    [Header("Water Timers")]
    public float waterNeedInterval;
    private float waterTimer;
    public float wateringTime;
    private float waterCompletionTimer;
    public float waterCompletionTime;

    [Header("Water bools")]
    public bool waterNeed;
    public bool waterMiniGameCompleted;
    
    [Header("Manure Timers")]
    public float manureNeedInterval;
    private float manureTimer;
    public float manureingTime;
    private float manureCompletionTimer;
    public float manureCompletionTime;

    [Header("Manure Bools")]
    public bool manureNeed;
    public bool manureMiniGameCompleted;

    [Header("GameObjects")]
    public GameObject waterIndicator;
    public GameObject manureIndicator;

    private void Start()
    {
        waterTimer = waterNeedInterval;
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
            waterTimer = waterNeedInterval;
            return;
        }

        if(manureNeed == true)
        {
            manureTimer -= Time.deltaTime;
            if(manureTimer <= 0)
            {
                ManureMiniGame();
            }
        }
        else
        {
            manureTimer = manureNeedInterval;
            return;
        }
    }
    private void WaterMiniGame()
    {
        waterIndicator.SetActive(true);        
        if(waterMiniGameCompleted == false)
        {
            waterCompletionTimer -= Time.deltaTime;
            if (waterCompletionTimer <= 0)
            {
                waterIndicator.SetActive(false);
                waterTimer = waterNeedInterval;
                waterCompletionTimer = waterCompletionTime;
                //subtract points
                Debug.Log("-points");
            }
        }
        else
        {
            waterIndicator.SetActive(false);
            waterTimer = waterNeedInterval;
            waterCompletionTimer = waterCompletionTime;
            waterMiniGameCompleted = false;
            //add points
            Debug.Log("+points");
        }
    }
    public void IsBeingWatered()
    {
        wateringTime -= Time.deltaTime;
        if(wateringTime <= 0)
        {
            waterMiniGameCompleted = true;
        }
    }
    private void ManureMiniGame()
    {
        manureIndicator.SetActive(true);
        if(manureMiniGameCompleted == false)
        {
            manureCompletionTimer -= Time.deltaTime;
            if(manureCompletionTimer <= 0)
            {
                manureIndicator.SetActive(false);
                manureTimer = manureNeedInterval;
                manureCompletionTimer = manureCompletionTime;
                //subtract points
                Debug.Log("-points");
            }
        }
        else
        {
            manureIndicator.SetActive(false);
            manureTimer = manureNeedInterval;
            manureCompletionTimer = manureCompletionTime;
            manureMiniGameCompleted = false;
            //+points
        }
    }
    public void IsBeingManured()
    {
        manureingTime -= Time.deltaTime;
        if(manureingTime <= 0)
        {
            manureMiniGameCompleted = true;
        }
    }

}
