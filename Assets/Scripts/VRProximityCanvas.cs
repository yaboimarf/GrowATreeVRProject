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

    [Header("Plant Object (Parent met 4 vormen erin)")]
    public Transform plantTransform;
    public Renderer plantRenderer;

    [Header("Tree Evolution (4 vormen als children)")]
    public GameObject treeStage1;  // sprout
    public GameObject treeStage2;  // small
    public GameObject treeStage3;  // medium
    public GameObject treeStage4;  // big

    [Header("Evolution Scale Thresholds")]
    public float evolveAtStage1 = 0.6f;
    public float evolveAtStage2 = 1.2f;
    public float evolveAtStage3 = 2f;

    private int currentStage = 1;

    [Header("Growth Settings")]
    public float growthPerPoint = 0.05f;
    public float maxScale = 3f;

    [Header("Color Settings")]
    public float colorChangeStep = 0.1f;

    [Header("Game Settings")]
    public int startPoints = 1;

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

    public int totalPoints = 1;
    private bool bonusActive = false;
    private bool fertilizerActive = false;

    private Material plantMaterial;

    private void Start()
    {
        totalPoints = startPoints;
        UpdatePointsUI();

        if (plantRenderer != null)
        {
            plantMaterial = plantRenderer.material;
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

    // ------------------------------
    // TREE EVOLUTION VIA SETACTIVE
    // ------------------------------
    private void CheckEvolution()
    {
        float scaleX = plantTransform.localScale.x;

        if (currentStage == 1 && scaleX >= evolveAtStage1)
            SwitchToStage(2);

        else if (currentStage == 2 && scaleX >= evolveAtStage2)
            SwitchToStage(3);

        else if (currentStage == 3 && scaleX >= evolveAtStage3)
            SwitchToStage(4);
    }

    private void SwitchToStage(int newStage)
    {
        currentStage = newStage;

        // Zet alle uit
        treeStage1.SetActive(false);
        treeStage2.SetActive(false);
        treeStage3.SetActive(false);
        treeStage4.SetActive(false);

        // Zet juiste boomvorm aan
        if (newStage == 1) treeStage1.SetActive(true);
        if (newStage == 2) treeStage2.SetActive(true);
        if (newStage == 3) treeStage3.SetActive(true);
        if (newStage == 4) treeStage4.SetActive(true);

        // Nieuwe renderer zoeken (belangrijk voor kleur)
        Renderer newRend = plantTransform.GetComponentInChildren<Renderer>();
        if (newRend != null)
        {
            plantRenderer = newRend;
            plantMaterial = newRend.material;
        }
    }

    // ------------------------------
    // PLANT GROEI (origineel)
    // ------------------------------
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

                CheckEvolution(); // ← Boomvorm checken
            }
        }
    }

    private IEnumerator PlantColorCoroutine()
    {
        if (plantMaterial == null)
            yield break;

        while (true)
        {
            yield return new WaitForSeconds(1f);
            Color targetColor = GetTargetColor();

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
            return new Color(1f, 0.5f, 0f);
        else
            return Color.black;
    }

    // ------------------------------
    // DE REST VAN JOUW ORIGINELE CODE
    // ------------------------------
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
        totalPoints = Mathf.Max(1, totalPoints + amount);
        UpdatePointsUI();
    }
}
