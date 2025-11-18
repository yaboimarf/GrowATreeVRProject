using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class VRPointsPlantWithBonusButton : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI pointsText;     // TextMeshPro tekst voor totaal punten
    public Button bonusButton;             // Speciale bonus button
    public Button fertilizerButton;        // Nieuwe bemestingsknop

    [Header("Plant Object")]
    public Transform plantTransform;       // Plant die groeit

    [Header("Growth Settings")]
    public float growthPerPoint = 0.05f;   // Groei per punt per seconde
    public float maxScale = 3f;            // Maximum schaal van de plant

    [Header("Bonus Button Settings")]
    public float bonusInterval = 10f;
    public float bonusDuration = 5f;
    public int bonusPoints = 5;
    public int penaltyPoints = 5;

    [Header("Fertilizer Button Settings")]
    public float fertilizerInterval = 15f;
    public float fertilizerDuration = 5f;
    public int fertilizerPoints = 3;       // Punten voor bemesting
    public int fertilizerPenalty = 3;      // Straf voor missen

    private int totalPoints = 0;

    private bool bonusActive = false;
    private bool fertilizerActive = false;

    private void Start()
    {
        // BONUS BUTTON
        if (bonusButton != null)
        {
            bonusButton.interactable = false;
            bonusButton.gameObject.SetActive(false);
            bonusButton.onClick.AddListener(OnBonusButtonClicked);
        }

        // FERTILIZER BUTTON
        if (fertilizerButton != null)
        {
            fertilizerButton.interactable = false;
            fertilizerButton.gameObject.SetActive(false);
            fertilizerButton.onClick.AddListener(OnFertilizerButtonClicked);
        }

        UpdatePointsUI();

        // Start coroutines
        StartCoroutine(GrowPlantCoroutine());
        StartCoroutine(BonusButtonCoroutine());
        StartCoroutine(FertilizerButtonCoroutine());
    }

    // GROEI COROUTINE
    private IEnumerator GrowPlantCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            if (plantTransform != null && totalPoints > 0)
            {
                float growthAmount = totalPoints * growthPerPoint;
                Vector3 newScale = plantTransform.localScale + Vector3.one * growthAmount;
                newScale = Vector3.Min(newScale, Vector3.one * maxScale);
                plantTransform.localScale = newScale;
            }
        }
    }

    // BONUS BUTTON COROUTINE
    private IEnumerator BonusButtonCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(bonusInterval);

            bonusActive = true;
            bonusButton.gameObject.SetActive(true);
            bonusButton.interactable = true;

            float timer = 0f;
            bool clicked = false;

            while (timer < bonusDuration)
            {
                if (!bonusActive)
                {
                    clicked = true;
                    break;
                }

                timer += Time.deltaTime;
                yield return null;
            }

            bonusButton.interactable = false;
            bonusButton.gameObject.SetActive(false);

            if (clicked)
                totalPoints += bonusPoints;
            else
                totalPoints = Mathf.Max(0, totalPoints - penaltyPoints);

            UpdatePointsUI();
            bonusActive = false;
        }
    }

    // BEMESTINGSBUTTON COROUTINE
    private IEnumerator FertilizerButtonCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(fertilizerInterval);

            fertilizerActive = true;
            fertilizerButton.gameObject.SetActive(true);
            fertilizerButton.interactable = true;

            float timer = 0f;
            bool clicked = false;

            while (timer < fertilizerDuration)
            {
                if (!fertilizerActive)
                {
                    clicked = true;
                    break;
                }

                timer += Time.deltaTime;
                yield return null;
            }

            fertilizerButton.interactable = false;
            fertilizerButton.gameObject.SetActive(false);

            if (clicked)
                totalPoints += fertilizerPoints;
            else
                totalPoints = Mathf.Max(0, totalPoints - fertilizerPenalty);

            UpdatePointsUI();
            fertilizerActive = false;
        }
    }

    // ONCLICK EVENTS
    private void OnBonusButtonClicked()
    {
        if (!bonusActive) return;
        bonusActive = false;
    }

    private void OnFertilizerButtonClicked()
    {
        if (!fertilizerActive) return;
        fertilizerActive = false;
    }

    private void UpdatePointsUI()
    {
        if (pointsText != null)
            pointsText.text = "Points: " + totalPoints;
    }
}
