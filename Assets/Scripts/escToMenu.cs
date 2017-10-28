using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class escToMenu : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown((KeyCode.Escape)))
        {
            SceneManager.LoadScene("MainMenu");
        }


    }
}


