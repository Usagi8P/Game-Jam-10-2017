using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeTransparent : MonoBehaviour {
    public LayerMask layerMask;
    Color color;
    // Use this for initialization
    void Start () {
        color = GetComponent<SpriteRenderer>().color; 
    }
	
	// Update is called once per frame
	void Update () {
		if (Physics.CheckSphere(transform.position + Vector3.forward, 0.5f, layerMask))
        {
            color.a = 0.5f;
            GetComponent<SpriteRenderer>().color = color;
        }
        else
        {
            color.a = 1f;
            GetComponent<SpriteRenderer>().color = color;
        }

	}
}
