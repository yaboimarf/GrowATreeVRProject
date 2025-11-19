using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class VRPointsPlantWithBonusButton : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI pointsText;
    public Button bonusButton;
    public Button fertilizerButton;

    [Header("Extra Point Buttons")]
    public Button addPointButton;
    public Button removePointButton;

    [Header("Plant Object")]
    public Transform plantTransform;
    public Renderer plantRenderer;

    [Header("Growth Settings")]
    public float growthPerPoint = 0.05f;
    public float maxScale = 3f;

    [Header("Color Settings")]
    public float colorChangeStep = 0.1f; // stap per seconde

    [Header("Game Settings")]
    public int startPoints = 0; // startpunten instelbaar in Inspector

    [Header("Bonus Button Settings")]
    public float bonusInterval = 10f;
    public float bonusDuration = 5f;
    public int bonusPoints = 5;
    public int penaltyPoints = 5;

    [Header("Fertilizer Button Settings")]
    public float fertilizerInterval = 15f;
    public float fertilizerDuration = 5f;
    public int fertilizerPoints = 3;
    public int fertilizerPenalty = 3;

    private int totalPoints = 0;

    private bool bonusActive = false;
    private bool fertilizerActive = false;

    private void Start()
    {
        // Startpunten instellen
        totalPoints = startPoints;
        UpdatePointsUI();

        // Kleur direct instellen op basis van startPoints
        if (plantRenderer != null)
        {
            if (totalPoints > 75)
                plantRenderer.material.color = Color.green;
            else if (totalPoints > 30)
                plantRenderer.material.color = new Color(1f, 0.5f, 0f); // Oranje
            else
                plantRenderer.material.color = Color.black;
        }

        // Bonus- en fertilizer-buttons
        if (bonusButton != null)
        {
            bonusButton.interactable = false;
            bonusButton.gameObject.SetActive(false);
            bonusButton.onClick.AddListener(OnBonusButtonClicked);
        }

        if (fertilizerButton != null)
        {
            fertilizerButton.interactable = false;
            fertilizerButton.gameObject.SetActive(false);
            fertilizerButton.onClick.AddListener(OnFertilizerButtonClicked);
        }

        // Extra +1/-1 knoppen
        if (addPointButton != null)
            addPointButton.onClick.AddListener(() =>
            {
                totalPoints += 1;
                UpdatePointsUI();
            });

        if (removePointButton != null)
            removePointButton.onClick.AddListener(() =>
            {
                totalPoints = Mathf.Max(0, totalPoints - 1);
                UpdatePointsUI();
            });

        StartCoroutine(GrowPlantCoroutine());
        StartCoroutine(BonusButtonCoroutine());
        StartCoroutine(FertilizerButtonCoroutine());
        StartCoroutine(PlantColorCoroutine());
    }

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

    private IEnumerator PlantColorCoroutine()
    {
        if (plantRenderer == null)
            yield break;

        while (true)
        {
            yield return new WaitForSeconds(1f);

            Color targetColor;

            if (totalPoints > 75)
                targetColor = Color.green;
            else if (totalPoints > 30)
                targetColor = new Color(1f, 0.5f, 0f); // Oranje
            else
                targetColor = Color.black;

            plantRenderer.material.color = Color.Lerp(
                plantRenderer.material.color,
                targetColor,
                colorChangeStep
            );
        }
    }

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
