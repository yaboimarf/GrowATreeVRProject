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
    public float colorChangeStep = 0.1f; // Stap per tick

    [Header("Game Settings")]
    public int startPoints = 1; // start altijd minimaal 1 punt

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

    private int totalPoints = 1;
    private bool bonusActive = false;
    private bool fertilizerActive = false;

    private Material plantMaterial;

    private void Start()
    {
        totalPoints = startPoints;
        UpdatePointsUI();

        if (plantRenderer != null)
        {
            plantMaterial = plantRenderer.material; // Unieke material instance
            plantMaterial.color = GetTargetColor();
        }

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

        if (addPointButton != null)
            addPointButton.onClick.AddListener(() => { AddPoints(1); });

        if (removePointButton != null)
            removePointButton.onClick.AddListener(() => { AddPoints(-1); });

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
        if (plantMaterial == null)
            yield break;

        while (true)
        {
            yield return new WaitForSeconds(1f); // 1 tick per seconde

            Color targetColor = GetTargetColor();

            // Kleine stap richting target per tick
            plantMaterial.color = Color.Lerp(
                plantMaterial.color,
                targetColor,
                colorChangeStep
            );
        }
    }

    private Color GetTargetColor()
    {
        if (totalPoints > 75)
            return Color.green;
        else if (totalPoints > 30)
            return new Color(1f, 0.5f, 0f); // Oranje
        else
            return Color.black;
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
                AddPoints(bonusPoints);
            else
                AddPoints(-penaltyPoints);

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
                AddPoints(fertilizerPoints);
            else
                AddPoints(-fertilizerPenalty);

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

    public void AddPoints(int amount)
    {
        totalPoints = Mathf.Max(1, totalPoints + amount); // minimaal 1 punt
        UpdatePointsUI();
    }
}
