using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomb : MonoBehaviour {

	public GameObject Bomb;

	// Use this for initialization
	void Start () {
		var bomb = Instantiate (Bomb);
		bomb.transform.position = new Vector2(5, 5);
		bomb.transform.localScale = new Vector2(5,5);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
