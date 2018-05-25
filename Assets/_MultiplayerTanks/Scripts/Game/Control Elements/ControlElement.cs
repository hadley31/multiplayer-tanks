using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ControlElement : MonoBehaviour
{
	public int ControlIndex
	{
		get { return Universe.controls.IndexOf (this); }
	}

	public bool InControl
	{
		get { return Universe.ElementInControl == this; }
	}

	protected virtual void OnEnable ()
	{
		Universe.controls.Add (this);
		Debug.Log (name + " has been added to controls");
	}

	protected virtual void OnDisable ()
	{
		Universe.controls.Remove (this);
		Debug.Log (name + " has been removed from controls");
	}

	public abstract void OnGainControl ();

	public abstract void OnLoseControl ();

	public abstract void OnControlUpdate ();
}
