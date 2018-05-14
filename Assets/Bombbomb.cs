using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bombbomb : MonoBehaviour {


	public GameObject ExplosionPreFab; 
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnDestroy(){
		Instantiate (ExplosionPreFab,transform.position,Quaternion.identity);
	}
}
