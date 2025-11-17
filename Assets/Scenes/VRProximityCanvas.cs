using UnityEngine;
using UnityEngine.UI;

public class VRCanvasCubeInteraction : MonoBehaviour
{
    [Header("Target Object")]
    public Transform targetObject; // De kubus die groeit

    [Header("Canvas")]
    public Canvas canvasToActivate;

    [Header("Settings")]
    public float activationDistance = 0.5f; // Afstand voor canvas

    [Header("XR Hand")]
    public Transform handTransform; // Hand/controller

    [Header("Cube Growth")]
    public float growthAmount = 0.1f; // Hoeveel de kubus groeit per klik

    [Header("UI Button")]
    public Button growButton; // Sleep hier de Button in de Inspector

    private void Start()
    {
        if (canvasToActivate != null)
            canvasToActivate.enabled = false;

        // Koppel de knop automatisch aan GrowCube functie
        if (growButton != null)
            growButton.onClick.AddListener(GrowCube);
    }

    private void Update()
    {
        if (targetObject == null || canvasToActivate == null || handTransform == null)
            return;

        float distance = Vector3.Distance(handTransform.position, targetObject.position);
        canvasToActivate.enabled = distance <= activationDistance;
    }

    // Functie die de kubus groter maakt
    public void GrowCube()
    {
        if (targetObject != null)
        {
            targetObject.localScale += Vector3.one * growthAmount;
        }
    }
}
