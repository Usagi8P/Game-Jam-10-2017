using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour {
    public Text score1;
    public Text score2;
    public Text Time_Left;
    private float Stopwatch;

    private int p1Score;
    private int p2Score;
    private void Start()
    {
        if (PlayerPrefs.GetFloat("Stopwatch") < 30)
        {
            Stopwatch = 30;
        }
        else
        {
            Stopwatch = PlayerPrefs.GetFloat("Stopwatch");
        }


    }

    private void Update()
    {
        score1.text = "HUNTER : " + p1Score.ToString();
        score2.text = "NECROMANCER : " + p2Score.ToString();

        Stopwatch -= Time.deltaTime;
        Time_Left.text = Stopwatch.ToString("0.0");
        if (Stopwatch < 0)
        {
            if (p1Score > p2Score)
            {
                SceneManager.LoadScene("HunterEnd");
                PlayerPrefs.SetInt("p1Score", p1Score);
                PlayerPrefs.SetInt("p2Score", p2Score);
            }
            else if (p1Score < p2Score)
            {
                SceneManager.LoadScene("NecroEnd");
                PlayerPrefs.SetInt("p1Score", p1Score);
                PlayerPrefs.SetInt("p2Score", p2Score);
            }
            else
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
    }
    public void SetP1Score(int _score)
    {
        p1Score = _score;
    }

    public void SetP2Score(int _score)
    {
        p2Score = _score;
    }
    public int GetP1Score()
    {
        return p1Score;
    }
    public int GetP2Score()
    {
        return p2Score;
    }
}
