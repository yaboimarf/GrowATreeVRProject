using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WinLoseController : MonoBehaviour
{
    [Header("Game References")]
    public float timer = 300f;           // 5 minuten
    public int punten = 0;               // Puntentelling
    public float boomHeight = 0f;        // Hoogte van de boom

    [Header("UI")]
    public GameObject winScreen;         // Win screen image
    public GameObject loseScreen;        // Lose screen image
    public TMP_Text timerText;           // Timer TextMeshPro

    private bool gameEnded = false;

    private void Start()
    {
        // Zorg dat beide uit staan aan het begin
        if (winScreen != null) winScreen.SetActive(false);
        if (loseScreen != null) loseScreen.SetActive(false);
    }

    private void Update()
    {
        if (gameEnded)
            return;

        // Timer aftellen
        timer -= Time.deltaTime;
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timer / 60f);
            int seconds = Mathf.FloorToInt(timer % 60f);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        // Timer voorbij?
        if (timer <= 0f)
        {
            timer = 0f;
            EndGame();
        }
    }

    private void EndGame()
    {
        gameEnded = true;

        // Controleer winst of verlies
        bool win = punten >= 70 && boomHeight > 3.5f;

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
    }
}
