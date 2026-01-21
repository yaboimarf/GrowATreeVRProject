using UnityEngine;
using UnityEngine.SceneManagement;

public class Play : MonoBehaviour
{
    // Sleep hier je play-object in
    public GameObject playObject;

    // Deze functie kan je direct koppelen aan XR Grab Interactable → On Select Enter
    public void OnGrabbed()
    {
        PlayGame();
    }

    private void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
