using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VRGrowObject : MonoBehaviour
{
    [Header("Scale Settings")]
    public Vector3 scaleIncrease = new Vector3(0.1f, 0.1f, 0.1f);

    [Header("Optional XR Interactable")]
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable interactable; // Sleep hier een interactable in als je wilt

    private void OnEnable()
    {
        if (interactable != null)
        {
            interactable.selectEntered.AddListener(OnSelectEntered);
        }
    }

    private void OnDisable()
    {
        if (interactable != null)
        {
            interactable.selectEntered.RemoveListener(OnSelectEntered);
        }
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        Grow();
    }

    public void Grow()
    {
        transform.localScale += scaleIncrease;
    }
}
