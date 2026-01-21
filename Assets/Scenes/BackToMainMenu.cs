using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMainMenu : MonoBehaviour
{
    [Header("Main Menu Scene Name")]
    public string mainMenuSceneName = "MainMenu";

    // Sleep hier je back-object in (optioneel, alleen voor referentie)
    public GameObject backObject;

    // Deze functie kan je koppelen aan XR Grab Interactable → On Select Enter
    public void OnGrabbed()
    {
        GoBack();
    }

    private void GoBack()
    {
        SceneManager.LoadSceneAsync(mainMenuSceneName);
    }
}
