using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowScores : MonoBehaviour {

    public Text p1Score;
    public Text p2Score;

    void Start()
    {
        p1Score.text = PlayerPrefs.GetInt("p1Score").ToString();
        p2Score.text = PlayerPrefs.GetInt("p2Score").ToString();

    }
}
