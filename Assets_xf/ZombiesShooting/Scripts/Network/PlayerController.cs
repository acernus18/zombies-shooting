using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Photon.MonoBehaviour
{
	private Vector3 currentPosition;
	private Quaternion currentRotation;

	public int health = 10;
	public GameObject hit;
	public GameObject death;
	public GameObject bullet;
	public GameObject bombPrefab;



	[PunRPC]
	public void takeDamage(int damage, int ownerId)
	{
		if (photonView.isMine && ownerId != photonView.ownerId)
		{
			health -= damage;
			PhotonNetwork.Instantiate("ParPlayerHitSplatter", transform.position, Utils.EffectRotation(), 0);
			if (health < 1)
			{
				PhotonNetwork.Instantiate("ParPlayerDeathSplatter", transform.position, Utils.EffectRotation(), 0);
				PhotonNetwork.Destroy(gameObject);
			}
		}
	}

	public void Awake()
	{
		GetComponent<PhotonView>().synchronization = ViewSynchronization.UnreliableOnChange;
		GetComponent<PhotonView>().ObservedComponents[0] = this;
		currentPosition = transform.position;
		currentRotation = transform.rotation;
	}

	public void Update()
	{

		if (Input.GetKeyDown (KeyCode.Space)) {
			Vector2 pos = transform.position;
			Instantiate (bombPrefab, pos, Quaternion.identity);
		}

		if (photonView.isMine)
		{
			float x = transform.position.x;
			float y = transform.position.y;
			float z = Camera.main.transform.position.z;

			Camera.main.transform.position = new Vector3(x, y, z);

			if (Input.GetMouseButtonDown(0))
			{
				GameObject bullet = PhotonNetwork.Instantiate("Bullet", transform.position, transform.rotation, 0);
				Physics2D.IgnoreCollision(GetComponent<Collider2D>(), bullet.GetComponent<Collider2D>());
			}
		}
	}

	public void FixedUpdate()
	{
		if (photonView.isMine)
		{
			Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			transform.rotation = Quaternion.LookRotation(Vector3.forward, transform.position - camRay.origin);
			transform.position += new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0).normalized * 0.1f;
		}
		else
		{
			transform.position = Vector3.Lerp(transform.position, currentPosition, Time.deltaTime * 5f);
			transform.rotation = Quaternion.Lerp(transform.rotation, currentRotation, Time.deltaTime * 5f);
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
		}
		else
		{
			currentPosition = (Vector3) stream.ReceiveNext();
			currentRotation = (Quaternion) stream.ReceiveNext();
		}
	}
}
