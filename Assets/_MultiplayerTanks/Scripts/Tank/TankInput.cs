using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;

public class TankInput : TankBase
{
	private const float Tank_Bottom_Height = 0.333f;
	private const float SQRT2 = 1.41421356237f;

	public static bool InputOverride = false;

	public string inputType = "keyboard";
	public float mouseSensitivity = 5.0f;

	private Plane m_GroundPlane;


	public Vector3 CursorPosition
	{
		get;
		set;
	}

	public Vector3 TargetCursorPosition
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
		if ( Tank.IsLocal == false )
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


		if ( inputType == "keyboard" )
		{
			Movement.SetBoostHeld (Keyboard.current.spaceKey.isPressed);

			if ( Mouse.current.leftButton.wasJustPressed )
			{
				Shooting.Shoot ();
			}

			if ( Mouse.current.rightButton.wasJustPressed )
			{
				TankLandmine.Use ();
			}
		}
		else if ( inputType == "controller" )
		{
			Movement.SetBoostHeld (Gamepad.current.leftStickButton.isPressed);

			if ( Gamepad.current.rightShoulder.wasJustPressed )
			{
				Shooting.Shoot ();
			}

			if ( Gamepad.current.leftShoulder.wasJustPressed )
			{
				TankLandmine.Use ();
			}
		}
		
	}

	private void UpdateCursorPosition ()
	{
		if ( inputType == "keyboard" )
		{
			Vector3 tempCursorPosition = TargetCursorPosition;

			tempCursorPosition += (Vector3) Mouse.current.delta.ReadValue () * mouseSensitivity;

			tempCursorPosition.x = Mathf.Clamp (tempCursorPosition.x, 0, Screen.width);
			tempCursorPosition.y = Mathf.Clamp (tempCursorPosition.y, 0, Screen.height);

			TargetCursorPosition = tempCursorPosition;

			CursorPosition =  Vector3.Lerp (CursorPosition, TargetCursorPosition, Time.deltaTime * 30.0f);
		}
		else if ( inputType == "controller" )
		{
			CursorPosition = Vector3.zero;
		}
	}

	public Vector3 GetLookTarget ()
	{
		if ( inputType == "keyboard" )
		{
			Ray ray = Camera.main.ScreenPointToRay (CursorPosition);

			float enterPoint;
			if ( m_GroundPlane.Raycast (ray, out enterPoint) )
			{
				return ray.GetPoint (enterPoint);
			}
		}
		else if ( inputType == "controller" )
		{
			Vector2 input = Gamepad.current.rightStick.ReadValue ();

			return transform.position + new Vector3 (input.x, 0, input.y);
		}
		

		return Vector3.zero;
	}

	private Vector3 GetTargetDirection ()
	{
		if ( inputType == "keyboard" )
		{
			Vector3 input = Vector3.zero;

			if (Keyboard.current.dKey.isPressed)
			{
				input.x++;
			}
			if (Keyboard.current.aKey.isPressed)
			{
				input.x--;
			}
			if (Keyboard.current.wKey.isPressed)
			{
				input.z++;
			}
			if (Keyboard.current.sKey.isPressed)
			{
				input.z--;
			}

			return input.normalized;
		}
		else if ( inputType == "controller" )
		{
			Vector2 input = Gamepad.current.leftStick.ReadValue ();

			return new Vector3 (input.x, 0, input.y);
		}

		return Vector3.zero;
	}
}