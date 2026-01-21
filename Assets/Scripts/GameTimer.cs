using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameTimerWinLose : MonoBehaviour
{
    [Header("Timer Settings")]
    public float totalTime = 300f; // 5 minuten
    public TMP_Text timerText;

    [Header("Win Conditions")]
    public int puntenThreshold = 70;
    public float boomScaleThreshold = 3.5f;

    [Header("UI Screens (GameScene)")]
    public GameObject winScreen;
    public GameObject loseScreen;
    public GameObject backButton; // 🔙 BACK BUTTON

    [Header("Scene Names")]
    public string winSceneName = "WinScene";
    public string loseSceneName = "LoseScene";
    public string mainMenuSceneName = "MainMenu";

    [Header("Scene Load Settings")]
    public bool autoLoadScene = false; // UIT laten als je eerst Back wilt klikken
    public float delayBeforeSceneLoad = 3f;

    [Header("References")]
    public VRPointsPlantWithBonusButton plantScript;

    private float currentTime;
    private bool gameEnded = false;

    private void Start()
    {
        currentTime = totalTime;

        // Alles UIT bij start
        if (winScreen != null) winScreen.SetActive(false);
        if (loseScreen != null) loseScreen.SetActive(false);
        if (backButton != null) backButton.SetActive(false);

        if (plantScript == null)
        {
            Debug.LogError("❌ PlantScript reference ontbreekt!");
        }
    }

    private void Update()
    {
        if (gameEnded) return;

        // Timer aftellen
        currentTime -= Time.deltaTime;
        if (currentTime < 0f) currentTime = 0f;

        // Timer UI
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60f);
            int seconds = Mathf.FloorToInt(currentTime % 60f);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }

        // Timer afgelopen
        if (currentTime <= 0f)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        gameEnded = true;

        if (plantScript == null || plantScript.plantTransform == null)
        {
            Debug.LogError("❌ Kan Win/Lose niet bepalen");
            return;
        }

        int points = plantScript.totalPoints;
        float boomHeight = plantScript.plantTransform.localScale.y;

        bool hasEnoughPoints = points >= puntenThreshold;
        bool treeIsBigEnough = boomHeight >= boomScaleThreshold;

        // Back button altijd tonen bij einde
        if (backButton != null)
            backButton.SetActive(true);

        if (hasEnoughPoints && treeIsBigEnough)
        {
            // ✅ WIN
            if (winScreen != null) winScreen.SetActive(true);
            if (loseScreen != null) loseScreen.SetActive(false);

            if (autoLoadScene)
                StartCoroutine(LoadSceneAfterDelay(winSceneName));
        }
        else
        {
            // ❌ LOSE
            if (loseScreen != null) loseScreen.SetActive(true);
            if (winScreen != null) winScreen.SetActive(false);

            if (autoLoadScene)
                StartCoroutine(LoadSceneAfterDelay(loseSceneName));
        }
    }

    // 🔙 Wordt aangeroepen door Button OnClick()
    public void GoBackToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    private IEnumerator LoadSceneAfterDelay(string sceneName)
    {
        yield return new WaitForSeconds(delayBeforeSceneLoad);
        SceneManager.LoadScene(sceneName);
    }
}
