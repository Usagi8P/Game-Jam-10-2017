using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour {
    public Text score1;
    public Text score2;

    private int p1Score;
    private int p2Score;

    private void Update()
    {
        score1.text = "Player 1 : " + p1Score.ToString();
        score2.text = "Player 2 : " + p2Score.ToString();


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
