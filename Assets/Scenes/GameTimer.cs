using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // TextMeshPro namespace

public class GameTimer : MonoBehaviour
{
    [Header("Timer Settings")]
    public float totalTime = 300f; // 5 minuten in seconden
    public string winLoseSceneName = "WinLose"; // Naam van de Win/Lose scene

    [Header("UI (optional)")]
    public TMP_Text timerText; // TextMeshPro component om de timer te tonen

    private float currentTime;

    private void Start()
    {
        currentTime = totalTime;
    }

    private void Update()
    {
        // Timer aftellen
        currentTime -= Time.deltaTime;

        // Update TMP UI als aanwezig
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60f);
            int seconds = Mathf.FloorToInt(currentTime % 60f);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        // Timer voorbij?
        if (currentTime <= 0f)
        {
            currentTime = 0f;
            GoToWinLose();
        }
    }

    private void GoToWinLose()
    {
        // Laad de Win/Lose scene
        SceneManager.LoadScene(winLoseSceneName);
    }
}
