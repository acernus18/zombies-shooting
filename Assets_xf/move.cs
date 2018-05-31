using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour {


	public float speed = 16f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate(){
		
		float h = Input.GetAxisRaw ("Horizontal");
		float v = Input.GetAxisRaw ("Vertical");

		GetComponent<Rigidbody2D> ().velocity = new Vector2 (h, v) * speed;

		//播放动画
		GetComponent<Animator>().SetInteger("X",(int)h);
		GetComponent<Animator>().SetInteger("Y",(int)v);

	}
}
