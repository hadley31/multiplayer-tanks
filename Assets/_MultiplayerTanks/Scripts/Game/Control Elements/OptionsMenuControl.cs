using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenuControl : ControlElement
{

	public override void OnGainControl ()
	{
		TankInput.InputOverride = true;
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	public override void OnControlUpdate ()
	{

	}

	public override void OnLoseControl ()
	{
		TankInput.InputOverride = false;
	}
}
