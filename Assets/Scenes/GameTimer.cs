using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameTimerWinLose : MonoBehaviour
{
    [Header("Timer Settings")]
    public float totalTime = 300f; // 5 minuten
    public TMP_Text timerText;     // Timer UI (TextMeshPro)

    [Header("Win/Lose Settings")]
    public int puntenThreshold = 70;          // Minimale punten voor winst
    public float boomScaleThreshold = 3.5f;   // Minimale boomhoogte voor winst
    public GameObject winScreen;              // Win screen UI
    public GameObject loseScreen;             // Lose screen UI
    public bool autoLoadScene = true;         // Wil je automatisch naar een scene gaan?
    public float delayBeforeSceneLoad = 3f;   // Wacht 3 seconden voor scene switch
    public string winLoseSceneName = "WinLose"; // Scene naam

    [Header("References")]
    public VRPointsPlantWithBonusButton plantScript; // Script met punten en boom info

    private float currentTime;
    private bool gameEnded = false;

    private void Start()
    {
        currentTime = totalTime;

        // Zorg dat beide screens uitstaan
        if (winScreen != null) winScreen.SetActive(false);
        if (loseScreen != null) loseScreen.SetActive(false);

        if (plantScript == null)
        {
            Debug.LogWarning("PlantScript niet ingesteld! Win/Lose check zal niet correct werken.");
        }
    }

    private void Update()
    {
        if (gameEnded) return;

        // Timer aftellen
        currentTime -= Time.deltaTime;
        if (currentTime < 0f) currentTime = 0f;

        // Update timer UI
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60f);
            int seconds = Mathf.FloorToInt(currentTime % 60f);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        // Timer afgelopen?
        if (currentTime <= 0f)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        gameEnded = true;

        if (plantScript == null)
        {
            Debug.LogWarning("Geen plantScript ingesteld. Win/Lose check kan niet uitgevoerd worden!");
            return;
        }

        // Huidige punten en boomhoogte
        int currentPoints = plantScript.totalPoints;
        float boomHeight = plantScript.plantTransform != null ? plantScript.plantTransform.localScale.y : 0f;

        // Bepaal winst of verlies
        bool win = currentPoints >= puntenThreshold && boomHeight > boomScaleThreshold;

        if (win)
        {
            if (winScreen != null) winScreen.SetActive(true);
            if (loseScreen != null) loseScreen.SetActive(false);
        }
        else
        {
            if (loseScreen != null) loseScreen.SetActive(true);
            if (winScreen != null) winScreen.SetActive(false);
        }

        // Optioneel automatische scene switch
        if (autoLoadScene && !string.IsNullOrEmpty(winLoseSceneName))
        {
            StartCoroutine(LoadSceneAfterDelay(delayBeforeSceneLoad));
        }
    }

    private IEnumerator LoadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(winLoseSceneName);
    }
}
