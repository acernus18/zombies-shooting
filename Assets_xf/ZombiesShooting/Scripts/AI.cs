using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
	public static float ATTACK_DISTANCE = 1.2f;
	public static float TRACK_DISTANCE = 5.2f;

	public float speed = 0.02f;

	public enum State
	{
		IDLE,
		ATTACK,
		RUN
	}

	public State Last = State.IDLE;
	public State Current = State.IDLE;
	public GameObject Target;
	public bool Attacking = false;
	public IEnumerator Coroutine;

	private float GetDistance()
	{
		return Vector3.Distance(transform.position, Target.transform.position);
	}

	private void UpdateState()
	{
		var distance = GetDistance();

		Last = Current;
		if (distance < ATTACK_DISTANCE)
		{
			Current = State.ATTACK;
		}
		else if (distance < TRACK_DISTANCE && distance >= ATTACK_DISTANCE)
		{
			Current = State.RUN;
		}
		else
		{
			Current = State.IDLE;
		}
	}

	private void UpdateTransform()
	{
		if (Current == State.RUN)
		{
			transform.Translate((Target.transform.position - transform.position) * speed);

//			Vector3 force = Target.transform.position - transform.position;
//			GetComponent<Rigidbody2D>().AddForce(-transform.up * 0.1f);
		}

		if (Current != State.IDLE)
		{
			transform.rotation = Utils.Rotate(transform.position, Target.transform.position);
		}
	}
		
	// Update is called once per frame
	public void Update()
	{
		Target = GameObject.FindWithTag("Player");
		if (Target != null)
		{
			UpdateState();
			UpdateTransform();
			if (Current == State.ATTACK && Last != State.ATTACK)
			{
				StartCoroutine(Coroutine);
			}
			else if (Current != State.ATTACK && Last == State.ATTACK)
			{
				StopCoroutine(Coroutine);
			}
		}
		else
		{
			Last = Current;
			Current = State.IDLE;
		}
	}
}
