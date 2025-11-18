using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText; // Sleep hier je Text UI element in
    private int score = 0;

    void Start()
    {
        UpdateScoreText();
    }

    // Functie om +1 punt toe te voegen
    public void AddPoint()
    {
        score += 1;
        UpdateScoreText();
    }

    // Functie om -1 punt toe te geven
    public void SubtractPoint()
    {
        score -= 1;
        UpdateScoreText();
    }

    // Update het Text UI element
    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }
}
