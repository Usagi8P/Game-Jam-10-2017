using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {
    //string timer;

    public Text sliderValue;
    public Slider slider;

    void Update ()
    {
        sliderValue.text = "Seconds: " + slider.value.ToString("0");
        PlayerPrefs.SetFloat("Stopwatch", slider.value);
    }
    
}
