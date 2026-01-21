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
    public GameObject treeStage1;      // sprout
    public GameObject treeStage2;      // small
    public GameObject treeStage3;      // medium
    public GameObject treeStage4;      // big

    [Header("Dead Tree Stages (4 vormen als children)")]
    public GameObject treeDeadStage1;  // dead sprout
    public GameObject treeDeadStage2;  // dead small
    public GameObject treeDeadStage3;  // dead medium
    public GameObject treeDeadStage4;  // dead big

    [Header("Evolution Scale Thresholds")]
    public float evolveAtStage1 = 0.6f;
    public float evolveAtStage2 = 1.2f;
    public float evolveAtStage3 = 2f;

    private int currentStage = 1;
    [HideInInspector] public bool isDead = false;

    [Header("Growth Settings")]
    public float growthPerPoint = 0.05f;
    public float maxScale = 3f;

    [Header("Color Settings")]
    public float colorChangeStep = 0.1f;

    [Header("Game Settings")]
    public int startPoints = 50;
    public int totalPoints;

    private Material plantMaterial;

    // Reference naar GameTimerWinLose om lose screen te triggeren
    [Header("References")]
    public GameTimerWinLose gameTimerScript;

    private void Start()
    {
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

    private void CheckEvolution()
    {
        // Controleer dood
        if (!isDead && totalPoints < 40 && plantMaterial.color == Color.black)
        {
            SwitchToDeadStage(currentStage);
            isDead = true;

            // Start coroutine om 10 seconden te wachten en loseScreen te tonen
            if (gameTimerScript != null)
            {
                StartCoroutine(gameTimerScript.TriggerLoseAfterDelay(10f));
            }
            return;
        }

        if (isDead) return;

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
        Color currentColor = Color.white;
        if (plantMaterial != null)
            currentColor = plantMaterial.color;

        currentStage = newStage;

        treeStage1.SetActive(false);
        treeStage2.SetActive(false);
        treeStage3.SetActive(false);
        treeStage4.SetActive(false);
        treeDeadStage1.SetActive(false);
        treeDeadStage2.SetActive(false);
        treeDeadStage3.SetActive(false);
        treeDeadStage4.SetActive(false);

        if (newStage == 1) treeStage1.SetActive(true);
        if (newStage == 2) treeStage2.SetActive(true);
        if (newStage == 3) treeStage3.SetActive(true);
        if (newStage == 4) treeStage4.SetActive(true);

        Renderer newRend = plantTransform.GetComponentInChildren<Renderer>();
        if (newRend != null)
        {
            plantRenderer = newRend;
            plantMaterial = newRend.material;
            plantMaterial.color = currentColor;
        }
    }

    private void SwitchToDeadStage(int stage)
    {
        currentStage = stage;

        treeStage1.SetActive(false);
        treeStage2.SetActive(false);
        treeStage3.SetActive(false);
        treeStage4.SetActive(false);
        treeDeadStage1.SetActive(false);
        treeDeadStage2.SetActive(false);
        treeDeadStage3.SetActive(false);
        treeDeadStage4.SetActive(false);

        if (stage == 1) treeDeadStage1.SetActive(true);
        if (stage == 2) treeDeadStage2.SetActive(true);
        if (stage == 3) treeDeadStage3.SetActive(true);
        if (stage == 4) treeDeadStage4.SetActive(true);

        Renderer newRend = plantTransform.GetComponentInChildren<Renderer>();
        if (newRend != null)
        {
            plantRenderer = newRend;
            plantMaterial = newRend.material;
            plantMaterial.color = Color.black;
        }
    }

    private IEnumerator GrowPlantCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            if (plantTransform != null && totalPoints > 0 && !isDead)
            {
                float growthAmount = totalPoints * growthPerPoint;
                Vector3 newScale = plantTransform.localScale + Vector3.one * growthAmount;
                newScale = Vector3.Min(newScale, Vector3.one * maxScale);

                plantTransform.localScale = newScale;

                CheckEvolution();
            }
            else
            {
                CheckEvolution();
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
            if (isDead) continue;

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
        else if (totalPoints > 40)
            return new Color(1f, 0.5f, 0f);
        else
            return Color.black;
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
