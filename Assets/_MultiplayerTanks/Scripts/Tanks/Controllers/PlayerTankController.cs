using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTankController : TankController
{
	protected Plane groundPlane;

	protected Vector3 input;
	protected Vector3 velocity;

	protected override void Awake ()
	{
		base.Awake ();

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
		base.OnControlUpdate ();

		GetInput ();
	}

	protected virtual void FixedUpdate ()
	{
		if (InControl)
		{
			tank.Rotate (velocity, turnSpeed);
			tank.Move (velocity);
		}
	}

	protected void LateUpdate ()
	{
		if (InControl)
		{
			tank.Look (GetTargetPoint (), topRotateSpeed);
		}
	}


	protected override Vector3 GetTargetPoint ()
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
			speed = moveSpeed + moveSpeed * boostSpeedMultiplier * boost;
			boost -= Time.deltaTime * boostUseSpeed;
		}
		else if ( boost < maxBoost )
		{
			boost += Time.deltaTime * boostRegainSpeed;
		}
		boost = Mathf.Clamp (boost, 0, maxBoost);

		velocity = input * speed;
	}
}
