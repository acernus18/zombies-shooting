using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyAfter : MonoBehaviour {


	public float time = 3f;

	// Use this for initialization
	void Start () {
		Destroy (gameObject, time);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
