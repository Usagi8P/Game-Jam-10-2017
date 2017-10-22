using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class escToMenu : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown((KeyCode.Escape)))
            LoadByIndex(0);

    }

    public void LoadByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}


