using UnityEngine;
using System.Collections.Generic;
using TMPro;

public sealed class Universe : MonoBehaviour
{
	public static Universe Instance
	{
		get;
		private set;
	}

	#region Monobehaviours

	private void OnEnable ()
	{
		if ( Instance == null )
		{
			Instance = this;
			DontDestroyOnLoad (this);
		}
		else
		{
			Debug.LogWarning ("A Universe object already exists! Destroying new Universe!");
			Destroy (this);
		}
	}

	private void OnDisable ()
	{
		if ( Instance == this )
		{
			Instance = null;
		}
	}

	private void Update ()
	{
		ControlUpdate ();
	}

	#endregion

	#region Control Elements

	public static readonly List<ControlElement> controls = new List<ControlElement> ();

	public static ControlElement ElementInControl
	{
		get;
		private set;
	}

	private void ControlUpdate ()
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

	private ControlElement GetLastControl ()
	{
		return controls.Count > 0 ? controls[controls.Count - 1] : null;
	}

	#endregion
}
