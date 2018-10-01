using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ControlElement : MonoBehaviour
{
	public int ControlIndex
	{
		get { return Universe.controls.IndexOf (this); }
	}

	public bool HasControl
	{
		get { return Universe.ElementInControl == this; }
	}

	protected virtual void OnEnable ()
	{
		GiveControl ();
	}

	protected virtual void OnDisable ()
	{
		RevokeControl ();
	}

	public virtual void GiveControl ()
	{
		if (this.gameObject.activeSelf == false || this.enabled == false)
		{
			return;
		}

		Universe.controls.Add (this);
		Debug.Log (name + " has been added to controls");
	}

	public virtual void RevokeControl ()
	{
		Universe.controls.Remove (this);
		Debug.Log (name + " has been removed from controls");
	}

	public abstract void OnGainControl ();

	public abstract void OnLoseControl ();

	public abstract void OnControlUpdate ();
}
