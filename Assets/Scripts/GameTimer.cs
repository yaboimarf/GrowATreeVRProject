using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameTimerWinLose : MonoBehaviour
{
    [Header("Timer Settings")]
    public float totalTime = 300f;
    public TMP_Text timerText;

    [Header("Win Conditions")]
    public int puntenThreshold = 70;
    public float boomScaleThreshold = 3.5f;

    [Header("UI Screens (GameScene)")]
    public GameObject winScreen;
    public GameObject loseScreen;
    public GameObject backButton;

    [Header("Scene Names")]
    public string mainMenuSceneName = "MainMenu";

    [Header("Scene Load Settings")]
    public float delayBeforeMainMenu = 10f;

    [Header("References")]
    public VRPointsPlantWithBonusButton plantScript;

    private float currentTime;
    private bool gameEnded = false;

    private void Start()
    {
        currentTime = totalTime;

        if (winScreen != null) winScreen.SetActive(false);
        if (loseScreen != null) loseScreen.SetActive(false);
        if (backButton != null) backButton.SetActive(false);

        if (plantScript == null)
        {
            Debug.LogError("❌ PlantScript reference ontbreekt!");
        }
        else
        {
            plantScript.gameTimerScript = this;
        }
    }

    private void Update()
    {
        if (gameEnded) return;

        currentTime -= Time.deltaTime;
        if (currentTime < 0f) currentTime = 0f;

        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60f);
            int seconds = Mathf.FloorToInt(currentTime % 60f);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }

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

        if (backButton != null)
            backButton.SetActive(true);

        if (hasEnoughPoints && treeIsBigEnough)
        {
            if (winScreen != null) winScreen.SetActive(true);
            if (loseScreen != null) loseScreen.SetActive(false);
        }
        else
        {
            if (loseScreen != null) loseScreen.SetActive(true);
            if (winScreen != null) winScreen.SetActive(false);
        }

        StartCoroutine(GoToMainMenuAfterDelay());
    }

    public void GoBackToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    private IEnumerator GoToMainMenuAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeMainMenu);
        SceneManager.LoadScene(mainMenuSceneName);
    }

    // Nieuw: trigger lose screen na dood boom
    public IEnumerator TriggerLoseAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        gameEnded = true;

        if (backButton != null)
            backButton.SetActive(true);

        if (loseScreen != null) loseScreen.SetActive(true);
        if (winScreen != null) winScreen.SetActive(false);

        StartCoroutine(GoToMainMenuAfterDelay());
    }
}
