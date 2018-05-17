using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeybindToggle : MonoBehaviour
{
	public KeyCode key;
	public bool toggled = false;
	public bool hold = false;
	public GameObject toggleObject;

	private void Start ()
	{
		Toggle (toggled);
	}

	void Update ()
	{
		if ( hold )
		{
			if (Input.GetKey (key))
			{
				Toggle (true);
			}
			else
			{
				Toggle (false);
			}
		}
		else if ( Input.GetKeyDown (key) )
		{
			Toggle ();
		}
	}

	public void Toggle ()
	{
		toggled = !toggled;
		toggleObject.SetActive (toggled);
	}

	public void Toggle (bool value)
	{
		toggled = value;
		toggleObject.SetActive (toggled);
	}
}
