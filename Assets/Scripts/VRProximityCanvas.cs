using UnityEngine;
using TMPro;
using System.Collections;

public class VRPointsPlantWithBonusButton : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI pointsText;

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
    public int startPoints = 50;
    public int totalPoints;

    private Material plantMaterial;

    private void Start()
    {
        // Startpunten correct instellen
        totalPoints = startPoints;
        UpdatePointsUI();

        if (plantRenderer != null)
        {
            plantMaterial = plantRenderer.material;
            plantMaterial.color = GetTargetColor();
        }

        StartCoroutine(GrowPlantCoroutine());
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

        // Zet alle stages uit
        treeStage1.SetActive(false);
        treeStage2.SetActive(false);
        treeStage3.SetActive(false);
        treeStage4.SetActive(false);

        // Zet de juiste stage aan
        if (newStage == 1) treeStage1.SetActive(true);
        if (newStage == 2) treeStage2.SetActive(true);
        if (newStage == 3) treeStage3.SetActive(true);
        if (newStage == 4) treeStage4.SetActive(true);

        // Nieuwe renderer voor kleur
        Renderer newRend = plantTransform.GetComponentInChildren<Renderer>();
        if (newRend != null)
        {
            plantRenderer = newRend;
            plantMaterial = newRend.material;
        }
    }

    // ------------------------------
    // PLANT GROEI
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

                CheckEvolution(); // Boomvorm checken
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
    // UI UPDATE EN PUNTEN TOEVOEGEN
    // ------------------------------
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
