using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuControl : ControlElement
{

	public override void OnGainControl ()
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	public override void OnControlUpdate ()
	{

	}

	public override void OnLoseControl ()
	{
		
	}
}
