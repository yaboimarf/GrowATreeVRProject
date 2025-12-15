using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Play : MonoBehaviour
{
    // Start is called before the first frame update
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void OpenSettings()
    {
        SceneManager.LoadSceneAsync(2);
    }

    public void CloseSettings()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
