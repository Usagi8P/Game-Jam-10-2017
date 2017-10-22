using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Stop_watch : MonoBehaviour {

    public Text Time_Left;
    public float Stopwatch;
    public float P1Score;
    public float P2Score;

    void Start()
    {
        Stopwatch = PlayerPrefs.GetFloat("Stopwatch");      
    }


    // Update is called once per frame
    void Update () {
        P1Score = PlayerPrefs.GetFloat("P1Score");
        P2Score = PlayerPrefs.GetFloat("P2Score");
        Stopwatch -= Time.deltaTime;
        Time_Left.text = Stopwatch.ToString("0.0");
        if (Stopwatch < 0 )
        {
            if (P1Score > P2Score)
            {
                SceneManager.LoadScene(6);
            }
            else if (P1Score < P2Score)
            {
                SceneManager.LoadScene(5);
            }
            else
            {
                SceneManager.LoadScene(0);
            }
        }
	}
}
