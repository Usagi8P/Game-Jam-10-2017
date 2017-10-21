using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseRandomTile : MonoBehaviour {
    private SpriteRenderer sprite;
    public Sprite[] s;

	// Use this for initialization
	void Start () {
        sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = s[Mathf.RoundToInt(Random.Range(0f,15f))];
    }
	
	
}
