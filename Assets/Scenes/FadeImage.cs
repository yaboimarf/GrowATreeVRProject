using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeWithHold : MonoBehaviour
{
    [Header("UI Image")]
    public Image targetImage;  // Sleep hier je Image in

    [Header("Fade Settings")]
    public float interval = 10f;      // Tijd tussen het starten van elke fade
    public float fadeDuration = 2f;   // Tijd voor fade in én fade out
    public float holdDuration = 1f;   // Hoelang de alpha op max blijft
    public float maxAlpha = 1f;       // Max transparantie (0-1)

    private void Start()
    {
        if (targetImage == null)
        {
            Debug.LogError("Target Image is not assigned!");
            return;
        }

        // Zet alpha eerst op 0
        SetAlpha(0f);

        // Start fade loop
        StartCoroutine(FadeLoop());
    }

    private IEnumerator FadeLoop()
    {
        while (true)
        {
            // Wacht interval
            yield return new WaitForSeconds(interval);

            // Fade 0 → max
            yield return StartCoroutine(FadeToAlpha(maxAlpha, fadeDuration / 2f));

            // Houd maxAlpha voor holdDuration
            yield return new WaitForSeconds(holdDuration);

            // Fade max → 0
            yield return StartCoroutine(FadeToAlpha(0f, fadeDuration / 2f));
        }
    }

    private IEnumerator FadeToAlpha(float targetAlpha, float duration)
    {
        float startAlpha = targetImage.color.a;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, timer / duration);
            SetAlpha(alpha);
            yield return null;
        }

        SetAlpha(targetAlpha);
    }

    private void SetAlpha(float alpha)
    {
        Color c = targetImage.color;
        c.a = alpha;
        targetImage.color = c;
    }
}
