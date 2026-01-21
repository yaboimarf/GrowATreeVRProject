using UnityEngine;

public class QuitGame : MonoBehaviour
{
    // Deze functie kan je koppelen aan een XR Grab Interactable of andere trigger
    public void Quit()
    {
        Debug.Log("Game afsluiten...");

        // Sluit de game in een build
        Application.Quit();

        // In de Editor werkt Application.Quit() niet, dus voor testen:
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
