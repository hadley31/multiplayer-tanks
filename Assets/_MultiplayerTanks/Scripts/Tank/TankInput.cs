using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankInput : TankBase
{
	private const float Tank_Bottom_Height = 0.333f;
	private const float SQRT2 = 1.41421356237f;

	public static bool InputOverride = false;

	private Plane m_GroundPlane;


	public Vector3 CursorPosition
	{
		get;
		set;
	}

	private void Awake ()
	{
		CursorPosition = new Vector3 (Screen.width / 2, Screen.height / 2);
		m_GroundPlane = new Plane (Vector3.up, Vector3.up * Tank_Bottom_Height);
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

		if (InputOverride == true)
		{
			return;
		}

		UpdateCursorPosition ();

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

	private void UpdateCursorPosition ()
	{
		Vector3 tempCursorPosition = CursorPosition;

		tempCursorPosition.x += Input.GetAxis ("Mouse X") * 20f;
		tempCursorPosition.y += Input.GetAxis ("Mouse Y") * 20f;

		tempCursorPosition.x = Mathf.Clamp (tempCursorPosition.x, 0, Screen.width);
		tempCursorPosition.y = Mathf.Clamp (tempCursorPosition.y, 0, Screen.height);

		CursorPosition = tempCursorPosition;
	}

	public Vector3 GetLookTarget ()
	{
		Ray ray = Camera.main.ScreenPointToRay (CursorPosition);

		float enterPoint;
		if ( m_GroundPlane.Raycast (ray, out enterPoint) )
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