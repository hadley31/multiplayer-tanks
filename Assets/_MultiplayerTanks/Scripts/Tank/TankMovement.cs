using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : TankBase
{
	public Transform top;

	public float moveSpeed = 4;
	public float rotateSpeed = 10;
	public float lookSpeed = 50;

	public float boostSpeedMultiplier = 3;
	public float maxBoost = 1;
	public float boostUseSpeed = 3;
	public float boostRegainSpeed = 0.2f;

	private float m_speed;

	public float Boost
	{
		get;
		private set;
	}

	public bool IsBoosting
	{
		get;
		private set;
	}

	public Rigidbody Rigidbody
	{
		get;
		private set;
	}

	public Vector3 TargetDirection
	{
		get;
		private set;
	}

	public Vector3 TargetVelocity
	{
		get { return TargetDirection.normalized * m_speed; }
	}

	public Quaternion TargetLook
	{
		get;
		private set;
	}

	public Vector3 Velocity
	{
		get { return Rigidbody.velocity; }
		private set { Rigidbody.velocity = value; }
	}

	protected virtual void Awake ()
	{
		Rigidbody = GetComponent<Rigidbody> ();
	}

	protected virtual void Update ()
	{
		if (photonView.isMine == false)
		{
			return;
		}

		if (Tank.IsAlive == false)
		{
			return;
		}

		UpdateSpeed ();
		Look ();
	}

	protected virtual void FixedUpdate ()
	{
		if (photonView.isMine == false)
		{
			return;
		}

		if (Tank.IsAlive == false)
		{
			return;
		}

		Rotate ();
		Move ();
	}

	protected virtual void UpdateSpeed ()
	{
		m_speed = moveSpeed;
		if ( Input.GetKey (KeyCode.Space) )
		{
			m_speed = moveSpeed + moveSpeed * boostSpeedMultiplier * Boost;
			Boost -= Time.deltaTime * boostUseSpeed;
		}
		else if ( Boost < maxBoost )
		{
			Boost += Time.deltaTime * boostRegainSpeed;
		}
		Boost = Mathf.Clamp (Boost, 0, maxBoost);
	}

	protected virtual void Move ()
	{
		Rigidbody.AddForce (TargetVelocity - Velocity, ForceMode.VelocityChange);
	}

	protected virtual void Rotate ()
	{
		if ( TargetDirection.sqrMagnitude > Mathf.Epsilon )
		{
			Quaternion target = Quaternion.LookRotation (TargetDirection, Vector3.up);
			Rigidbody.rotation = Quaternion.Slerp (transform.rotation, target, Time.fixedDeltaTime * rotateSpeed);
		}
	}

	protected virtual void Look ()
	{
		if ( TargetLook == Quaternion.identity )
			return;

		top.rotation = Quaternion.Slerp (top.rotation, TargetLook, Time.deltaTime * lookSpeed);
	}


	public void SetTargetDirection (Vector3 direction)
	{
		TargetDirection = direction;
	}

	public void SetLookTarget (float target)
	{
		TargetLook = Quaternion.Euler (0, target, 0);
	}

	public void SetLookTarget (Vector3 target)
	{
		Vector3 targetDirection = target - transform.position;
		targetDirection.y = 0;

		TargetLook = Quaternion.LookRotation (targetDirection);
	}
}
