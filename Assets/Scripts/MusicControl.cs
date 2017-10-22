using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicControl : MonoBehaviour {

    public AudioSource Necromancer;
    public AudioSource Hunter;
    float ZombieVol;
    float ZombieNumber;
    float Dead;

    // Use this for initialization
   void Start ()
    {

        ZombieNumber = GameObject.FindGameObjectsWithTag("zombie").Length;

    }
	
	// Update is called once per frame
	void Update () {

        Dead = 0;

        for (int i = 0; i < GameObject.FindGameObjectsWithTag("zombie").Length; i++)
        {
            if (GameObject.FindGameObjectsWithTag("zombie")[i].GetComponent<ZombieController>().isDead == true)
            {
                Dead++;
            }
        }

        ZombieVol = Dead / ZombieNumber;

        Necromancer.volume = 1 - ZombieVol;
        Hunter.volume = ZombieVol;
    }
}