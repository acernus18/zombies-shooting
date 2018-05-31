using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ZombieController : MonoBehaviour
{
	private Animator animator;
	private AI ai;
	private float moveSpeed = 0.1f;


	public int Health = 5;
	public GameObject Hit;
	public GameObject Death;


	private void Move(Vector3 direction)
	{
		float x = transform.position.x + direction.x;
		float y = transform.position.y + direction.y;
		transform.position = new Vector3(x, y) * moveSpeed;
	}

	private void UpdateAnimator()
	{
		switch (ai.Current)
		{
			case AI.State.ATTACK:
				animator.SetBool("run", false);
				animator.SetBool("attack", true);
				break;
			case AI.State.RUN:
				animator.SetBool("run", true);
				animator.SetBool("attack", false);
				break;
			case AI.State.IDLE:
				animator.SetBool("run", false);
				animator.SetBool("attack", false);
				break;
		}
	}

	private IEnumerator<WaitForSeconds> Attack()
	{
		while (true)
		{
			var controller = ai.Target.GetComponent<PlayerController>();
			Instantiate(controller.hit, ai.Target.transform.position, Utils.EffectRotation());
//			controller.takeDamage(1);
			yield return new WaitForSeconds(1);
		}
	}

	public void TakeDamage()
	{
		Health -= 1;

		if (Health < 0)
		{
			Instantiate(Death, transform.position, Utils.EffectRotation());
			Destroy(gameObject);
		}
	}

	public void Start()
	{
		animator = GetComponent<Animator>();
		ai = GetComponent<AI>();
		ai.Coroutine = Attack();

	}

	// Update is called once per frame
	public void Update()
	{
		UpdateAnimator();
	}


}
