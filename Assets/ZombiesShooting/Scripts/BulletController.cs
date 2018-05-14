using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : Photon.MonoBehaviour
{
	private float moveSpeed = 20f;

	private IEnumerator<WaitForSeconds> Travel()
	{
		yield return new WaitForSeconds(0.5f);

		if (photonView.isMine)
		{
			PhotonNetwork.Destroy(gameObject);
		}
	}

	public void Start()
	{
		StartCoroutine(Travel());
	}

	public void Update()
	{
		transform.position += transform.up * moveSpeed * Time.deltaTime * -1;
	}

	public void OnCollisionEnter2D(Collision2D other)
	{
		if (!photonView.isMine)
		{
			return;
		}

		if (other.gameObject.tag == "Player")
		{
			PhotonView otherPhotonView = other.gameObject.GetComponent<PhotonView>();
			Debug.Log(photonView.ownerId + " -> " + otherPhotonView.ownerId);
			PhotonNetwork.RPC(otherPhotonView, "takeDamage", PhotonTargets.All, false, 5, photonView.ownerId);
			PhotonNetwork.Destroy(gameObject);
		}
	}
}
