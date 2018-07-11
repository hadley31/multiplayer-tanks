using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankInput : TankBase
{
	private const float Tank_Bottom_Height = 0.11f;
	private const float SQRT2 = 1.41421356237f;

	private Plane groundPlane;

	private void Awake ()
	{
		groundPlane = new Plane (Vector3.up, Vector3.up * Tank_Bottom_Height);
	}


	private void Update ()
	{
		if ( photonView.isMine == false )
		{
			return;
		}

		if ( Tank.IsAlive == false )
		{
			return;
		}

		Movement.SetLookTarget (GetLookTarget ());
		Movement.SetTargetDirection (GetTargetDirection ());
		Movement.SetBoostHeld (Input.GetButton ("boost"));

		if ( Input.GetButtonDown ("shoot") )
		{
			Shooting.Shoot ();
		}

		if (Input.GetKeyDown (KeyCode.X))
		{
			TankLandmine.Use ();
		}
	}

	private Vector3 GetLookTarget ()
	{
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		float enterPoint;
		if ( groundPlane.Raycast (ray, out enterPoint) )
		{
			return ray.GetPoint (enterPoint);
		}

		return Vector3.zero;
	}

	private Vector3 GetTargetDirection ()
	{
		// Keyboard friendly input

		float vert = Input.GetAxisRaw ("Vertical");
		float horiz = Input.GetAxisRaw ("Horizontal");

		return new Vector3 (horiz, 0, vert).normalized;


		// Controller friendly input

		//float vert = Input.GetAxis ("Vertical");
		//float horiz = Input.GetAxis ("Horizontal");

		//return new Vector3 (horiz, 0, vert) / SQRT2;
	}
}