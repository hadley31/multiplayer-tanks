using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Axis
{
	public readonly string Name;

	public Key Negative
	{
		get;
		set;
	}

	public Key Positive
	{
		get;
		set;
	}

	public float Sensitivity
	{
		get;
		set;
	}

	public float Gravity
	{
		get;
		set;
	}

	public float Magnitude
	{
		get;
		private set;
	}

	public float RawMagnitude
	{
		get { return Mathf.Sign (Magnitude); }
	}

	public Axis (string name, Key negative, Key positive, float sensitivity = 1, float gravity = 1)
	{
		this.Magnitude = 0;
		this.Name = name;
		this.Negative = negative;
		this.Positive = positive;
		this.Sensitivity = sensitivity;
		this.Gravity = gravity;
	}

	public void Update_Magnitude ()
	{
		float target = this.RawMagnitude;
		float dm = target == 0 ? Gravity : Sensitivity;
		this.Magnitude = Mathf.MoveTowards (this.Magnitude, RawMagnitude, dm * Time.deltaTime);
	}
}
