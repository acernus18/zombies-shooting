using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Photon.MonoBehaviour
{
	private Vector3 currentPosition;
	private Quaternion currentRotation;
	private Animator currentAnimator;

	private AudioSource shot;

	private int health = 2;

	public void OnCollisionEnter2D(Collision2D other)
	{
		if (photonView.isMine && other.gameObject.tag == "bullet")
		{
			PhotonView pv = other.gameObject.GetComponent<PhotonView>();

			if (pv.ownerId != photonView.ownerId)
			{
				health -= 1;
				PhotonNetwork.Instantiate("ParPlayerHitSplatter", transform.position, Utils.EffectRotation(), 0);
				if (health < 1)
				{
					PhotonNetwork.Instantiate("ParPlayerDeathSplatter", transform.position, Utils.EffectRotation(), 0);
					PhotonNetwork.Destroy(gameObject);
				}
			}
		}
	}

	public void Awake()
	{
		GetComponent<PhotonView>().synchronization = ViewSynchronization.UnreliableOnChange;
		GetComponent<PhotonView>().ObservedComponents[0] = this;
		currentPosition = transform.position;
		currentRotation = transform.rotation;
		currentAnimator = GetComponent<Animator>();
		shot = GetComponent<AudioSource>();
	}

	public void FixedUpdate()
	{
		if (photonView.isMine)
		{
			Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -8);

			if (Input.GetMouseButtonDown(0))
			{
				shot.Play();
				GameObject bullet = PhotonNetwork.Instantiate("Bullet", transform.position, transform.rotation, 0);
				Physics2D.IgnoreCollision(GetComponent<Collider2D>(), bullet.GetComponent<Collider2D>());
			}

			Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			transform.rotation = Quaternion.LookRotation(Vector3.forward, transform.position - camRay.origin);

			Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0).normalized * 0.05f;
			currentAnimator.SetBool("run", movement != Vector3.zero);
			transform.position += movement;
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
