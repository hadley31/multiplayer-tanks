using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelControl : ControlElement
{
	public static GameLevelControl Current
	{
		get;
		private set;
	}

	public override void OnGainControl ()
	{
		Debug.Log ("GameLevelControl gained control");
		Current = this;

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	public override void OnLoseControl ()
	{
		if (Current == this)
		{
			Current = null;
		}
	}

	public override void OnControlUpdate ()
	{
		
	}
}
