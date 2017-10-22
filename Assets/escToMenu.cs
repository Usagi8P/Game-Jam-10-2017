using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class escToMenu : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown("space"))
            print("space key was pressed");

    }

    public void LoadByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}



public class changeScene : MonoBehaviour
{

    
}