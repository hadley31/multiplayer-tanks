using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeybindToggle : MonoBehaviour
{
	public KeyCode key;
	public bool startActive = false;
	public bool hold = false;
	public GameObject toggleObject;

	public bool Toggled
	{
		get { return toggleObject.activeSelf; }
		set { toggleObject.SetActive (value); }
	}


	private void Start ()
	{
		Toggled = startActive;
	}

	void Update ()
	{
#if UNITY_WEBGL || UNITY_EDITOR
		KeyCode key = this.key == KeyCode.Escape ? KeyCode.F10 : this.key;
#endif
		if ( hold )
		{
			Toggled = Input.GetKey (key);
		}
		else if ( Input.GetKeyDown (key) )
		{
			Toggle ();
		}
	}

	public void Toggle ()
	{
		Toggled = !Toggled;
	}
}
