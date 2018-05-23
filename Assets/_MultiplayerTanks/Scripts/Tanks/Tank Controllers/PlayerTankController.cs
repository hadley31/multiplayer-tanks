using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTankController : TankController
{
	private static Plane groundPlane;

	protected Vector3 input;
	protected Vector3 velocity;

	protected override void Awake ()
	{
		base.Awake ();

		if ( groundPlane.normal == Vector3.zero )
		{
			groundPlane = new Plane (Vector3.up, Vector3.zero);
		}
	}

	protected override void Update ()
	{
		base.Update ();

		GetInput ();
	}

	protected void FixedUpdate ()
	{
		tank.Rotate (velocity, turnSpeed);
		tank.Move (velocity);
	}

	protected void LateUpdate ()
	{
		tank.Look (GetTargetPoint (), topRotateSpeed);
	}


	protected override Vector3 GetTargetPoint ()
	{
		Ray ray = GetComponent<Camera> ().ScreenPointToRay (Input.mousePosition);

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
