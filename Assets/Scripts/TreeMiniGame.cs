using UnityEngine;
using UnityEngine.UI;

public class TreeMiniGame : MonoBehaviour
{
    [Header("Scripts")]
    public Water water;
    public Manure manure;
    public VRPointsPlantWithBonusButton points;

    [Header("Water Timers")]
    public float waterNeedInterval;
    private float waterTimer;
    private float wateringTimer;
    public float wateringTime;
    private float waterCompletionTimer;
    public float waterCompletionTime;

    [Header("Water bools")]
    public bool waterNeed;
    public bool waterMiniGameCompleted;
    public bool isBeingWatered;
        
    [Header("Manure Timers")]
    public float manureNeedInterval;
    private float manureTimer;
    private float manuringTimer;
    public float manuringTime;
    private float manureCompletionTimer;
    public float manureCompletionTime;

    [Header("Manure Bools")]
    public bool manureNeed;
    public bool manureMiniGameCompleted;
    public bool isbeingManured;

    [Header("GameObjects")]
    public GameObject waterIndicator;    
    public GameObject wateringPoint;
    public GameObject manureIndicator;
    public GameObject manurePoint;

    [Header("Points")]
    public int waterPoints;
    public int manurePoints;
    public int trashPoints;

    private void Start()
    {
        //water setup
        waterTimer = waterNeedInterval;
        waterCompletionTimer = waterCompletionTime;
        wateringTimer = wateringTime;

        //manure setup
        manureTimer = manureNeedInterval;
        manureCompletionTimer = manureCompletionTime;
        manuringTimer = manuringTime;
    }
    private void Update()
    {
        //water trigger
        if(waterNeed == true)
        {
            waterTimer -= Time.deltaTime;
            if(waterTimer <= 0)
            {
                WaterMiniGame();
            }
            else
            {
                wateringTimer = wateringTime;
            }
        }
        else
        {
            waterTimer = waterNeedInterval;
            return;
        }

        if(isBeingWatered == true)
        {
            WateringCompletion();
        }

        //manure trigger
        if(manureNeed == true)
        {
            manureTimer -= Time.deltaTime;
            if(manureTimer <= 0)
            {
                ManureMiniGame();
            }
            else
            {
                manuringTimer = manuringTime;
            }
        }
        else
        {
            manuringTimer = manureNeedInterval;
            return;
        }

        if(isbeingManured == true)
        {
            ManuringCompletion();
        }
    }

    //Water minigame functionality
    private void WaterMiniGame()
    {
        waterIndicator.SetActive(true);
        wateringPoint.SetActive(true);
        if (waterMiniGameCompleted == false)
        {
            waterCompletionTimer -= Time.deltaTime;
            if (waterCompletionTimer <= 0)
            {
                waterIndicator.SetActive(false);
                wateringPoint.SetActive(false);
                waterTimer = waterNeedInterval;
                wateringTimer = wateringTime;
                waterCompletionTimer = waterCompletionTime;
                isBeingWatered = false;
                water.ResetGravity();
                points.AddPoints(-waterPoints);
                Debug.Log("-points water");
            }
        }
        else
        {
            waterIndicator.SetActive(false);
            wateringPoint.SetActive(false);
            waterTimer = waterNeedInterval;
            wateringTimer = wateringTime;
            waterCompletionTimer = waterCompletionTime;
            waterMiniGameCompleted = false;
            isBeingWatered = false;
            water.ResetGravity();
            points.AddPoints(waterPoints);
            Debug.Log("+points water");
        }
    }
    public void IsBeingWatered()
    {
        isBeingWatered = true;
    }

    private void WateringCompletion()
    {
        if(isBeingWatered == true)
        {
            wateringTimer -= Time.deltaTime;
            if (wateringTimer <= 0)
            {
                waterMiniGameCompleted = true;                
            }
        }
    }

    //Manure minigame functionality  
    private void ManureMiniGame()
    {
        manureIndicator.SetActive(true);
        manurePoint.SetActive(true);
        if(manureMiniGameCompleted == false)
        {
            manureCompletionTimer -= Time.deltaTime;
            if(manureCompletionTimer <= 0)
            {
                manureIndicator.SetActive(false);
                manurePoint.SetActive(false);
                manureTimer = manureNeedInterval;
                manuringTimer = manuringTime;
                manureCompletionTimer = manureCompletionTime;
                isbeingManured = false;
                manure.ResetGravity();
                points.AddPoints(-manurePoints);
                Debug.Log("-points manure");
            }
        }
        else
        {
            manureIndicator.SetActive(false);
            manurePoint.SetActive(false);
            manureTimer = manureNeedInterval;
            manuringTimer = manuringTime;
            manureCompletionTimer = manureCompletionTime;
            manureMiniGameCompleted = false;
            isbeingManured = false;
            manure.ResetGravity();
            points.AddPoints(manurePoints);
            Debug.Log("+points manure");
        }
    }
    public void IsBeingManured()
    {
        isbeingManured = true;
    }
    private void ManuringCompletion()
    {
        if (isbeingManured == true)
        {
            manuringTimer -= Time.deltaTime;
            if (manuringTimer <= 0)
            {
                manureMiniGameCompleted = true;
            }
        }
    }

    //trash deletion points addon
    public void TrashPoints()
    {
        points.totalPoints += trashPoints;
        Debug.Log("Trash points");
    }
}
