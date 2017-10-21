using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseRandomWall : MonoBehaviour {
    public GameObject spriteTop;
    public GameObject spriteBot;
    public Sprite[] s;

    // Use this for initialization
    void Start()
    {
        spriteTop.GetComponent<SpriteRenderer>().sprite = s[Mathf.RoundToInt(Random.Range(0f, 7f))];
        spriteBot.GetComponent<SpriteRenderer>().sprite = s[Mathf.RoundToInt(Random.Range(8f, 15f))];
    }
}
