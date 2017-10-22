using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MasterVolume : MonoBehaviour {

    public AudioMixer masterMixer;

    public void SetVolume(float VolumeLvl)
    {
        masterMixer.SetFloat("Volume", VolumeLvl);
    }
}
