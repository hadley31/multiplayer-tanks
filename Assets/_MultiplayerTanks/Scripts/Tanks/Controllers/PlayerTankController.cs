using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTankController : ControlElement
{
	public float moveSpeed = 1;
	public float turnSpeed = 10;
	public float topRotateSpeed = 10;

	public float boostSpeedMultiplier = 5;
	public float maxBoost = 1;
	public float boostUseSpeed = 3;
	public float boostRegainSpeed = 0.2f;

	[Space (10)]
	public UnityEvent onShoot;
	public UnityEvent onUseLandmine;

	protected Tank tank;
	protected Plane groundPlane;

	protected Vector3 input;
	protected Vector3 velocity;

	public float Boost
	{
		get;
		set;
	}

	protected virtual void Awake ()
	{
		this.tank = GetComponent<Tank> ();
		groundPlane = new Plane (Vector3.up, Vector3.zero);
	}

	public override void OnGainControl ()
	{
		
	}

	public override void OnLoseControl ()
	{
		
	}

	public override void OnControlUpdate ()
	{
		GetInput ();
		tank.Look (GetTargetPoint (), topRotateSpeed);

		if (Input.GetKeyDown (KeyCode.Mouse0))
		{
			onShoot.Invoke ();
		}

		if (Input.GetKeyDown (KeyCode.X))
		{
			onUseLandmine.Invoke ();
		}
	}

	protected virtual void FixedUpdate ()
	{
		if (InControl)
		{
			tank.Rotate (velocity, turnSpeed);
			tank.Move (velocity);
		}
	}


	protected virtual Vector3 GetTargetPoint ()
	{
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		float enterPoint;
		if ( groundPlane.Raycast (ray, out enterPoint) )
		{
			return ray.GetPoint (enterPoint);
		}

		return Vector3.zero;
	}

	protected virtual void GetInput ()
	{
		float vert = Input.GetAxisRaw ("Vertical");
		float horiz = Input.GetAxisRaw ("Horizontal");

		input = new Vector3 (horiz, 0, vert).normalized;

		float speed = moveSpeed;
		if ( Input.GetKey (KeyCode.Space) )
		{
			speed = moveSpeed + moveSpeed * boostSpeedMultiplier * Boost;
			Boost -= Time.deltaTime * boostUseSpeed;
		}
		else if ( Boost < maxBoost )
		{
			Boost += Time.deltaTime * boostRegainSpeed;
		}
		Boost = Mathf.Clamp (Boost, 0, maxBoost);

		velocity = input * speed;
	}
}
