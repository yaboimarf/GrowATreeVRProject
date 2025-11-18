using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class VRPointsPlantWithBonusButton : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI pointsText;   // TextMeshPro tekst voor totaal punten
    public Button bonusButton;           // Speciale bonus button

    [Header("Plant Object")]
    public Transform plantTransform;     // Plant die groeit

    [Header("Growth Settings")]
    public float growthPerPoint = 0.05f; // Groei per punt per seconde
    public float maxScale = 3f;          // Maximum schaal van de plant

    [Header("Bonus Button Settings")]
    public float bonusInterval = 10f;    // Elke x seconden verschijnt de bonus button
    public float bonusDuration = 5f;     // Hoe lang de button klikbaar is
    public int bonusPoints = 5;          // Punten voor klikken
    public int penaltyPoints = 5;        // Punten voor missen

    private int totalPoints = 0;
    private bool bonusActive = false;

    private void Start()
    {
        if (bonusButton != null)
        {
            bonusButton.interactable = false;
            bonusButton.gameObject.SetActive(false);
            bonusButton.onClick.AddListener(OnBonusButtonClicked);
        }

        UpdatePointsUI();

        // Start coroutines
        StartCoroutine(GrowPlantCoroutine());
        StartCoroutine(BonusButtonCoroutine());
    }

    private IEnumerator GrowPlantCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // 1 tick per seconde

            if (plantTransform != null && totalPoints > 0)
            {
                float growthAmount = totalPoints * growthPerPoint;
                Vector3 newScale = plantTransform.localScale + Vector3.one * growthAmount;
                newScale = Vector3.Min(newScale, Vector3.one * maxScale);
                plantTransform.localScale = newScale;
            }
        }
    }

    private IEnumerator BonusButtonCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(bonusInterval);

            // Activeer button
            bonusActive = true;
            bonusButton.interactable = true;
            bonusButton.gameObject.SetActive(true);

            float timer = 0f;
            bool clicked = false;

            // Wacht tot bonusDuration of tot er geklikt wordt
            while (timer < bonusDuration)
            {
                if (!bonusActive) // als geklikt
                {
                    clicked = true;
                    break;
                }

                timer += Time.deltaTime;
                yield return null;
            }

            // Button uitschakelen
            bonusButton.interactable = false;
            bonusButton.gameObject.SetActive(false);

            // Punten toekennen
            if (clicked)
            {
                totalPoints += bonusPoints;
            }
            else
            {
                totalPoints -= penaltyPoints;
                if (totalPoints < 0) totalPoints = 0;
            }

            UpdatePointsUI();
            bonusActive = false;
        }
    }

    private void OnBonusButtonClicked()
    {
        if (!bonusActive) return;

        bonusActive = false; // voorkomt dubbele toekenning
    }

    private void UpdatePointsUI()
    {
        if (pointsText != null)
            pointsText.text = "Points: " + totalPoints;
    }
}
