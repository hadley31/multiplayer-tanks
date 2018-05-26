using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankInput : Photon.MonoBehaviour
{
	private Plane groundPlane;

	private TankMovement Movement
	{
		get;
		set;
	}

	private TankShoot Shooting
	{
		get;
		set;
	}

	private void Awake ()
	{
		Movement = GetComponent<TankMovement> ();
		Shooting = GetComponent<TankShoot> ();

		groundPlane = new Plane (Vector3.up, Vector3.zero);
	}


	private void Update ()
	{
		if (!photonView.isMine)
		{
			return;
		}


		Movement.SetLookTarget (GetLookTarget ());
		Movement.SetTargetDirection (GetTargetDirection ());


		if ( Input.GetKeyDown (KeyCode.Mouse0) )
		{
			Shooting.Shoot ();
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
		float vert = Input.GetAxisRaw ("Vertical");
		float horiz = Input.GetAxisRaw ("Horizontal");

		return new Vector3 (horiz, 0, vert).normalized;
	}
}