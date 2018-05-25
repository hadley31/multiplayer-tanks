using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubControlElement : ControlElement
{
	public List<ControlElement> controls = new List<ControlElement> ();

	public static ControlElement ElementInControl
	{
		get;
		private set;
	}

	public override void OnControlUpdate ()
	{
		ControlElement last = GetLastControl ();

		if ( ElementInControl == last && ElementInControl != null )
		{
			ElementInControl.OnControlUpdate ();
		}
		else
		{
			ElementInControl?.OnLoseControl ();
			last?.OnGainControl ();

			ElementInControl = last;
		}
	}

	public override void OnGainControl ()
	{

	}

	public override void OnLoseControl ()
	{

	}

	private ControlElement GetLastControl ()
	{
		return controls.Count > 0 ? controls[controls.Count - 1] : null;
	}
}
