using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion :Photon.MonoBehaviour{

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D coll){
		
		if (!coll.gameObject.isStatic) {
			Destroy (coll.gameObject);
		}

		if (coll.gameObject.tag == "Player")
		{
			PhotonView otherPhotonView = coll.gameObject.GetComponent<PhotonView>();
			Debug.Log(photonView.ownerId + " -> " + otherPhotonView.ownerId);
			PhotonNetwork.RPC(otherPhotonView, "takeDamage", PhotonTargets.All, false, 1, photonView.ownerId);
			PhotonNetwork.Destroy(gameObject);
		}

	}
}
