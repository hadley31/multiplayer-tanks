using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankBoostVisual : MonoBehaviour
{
	public Slider slider;

	public float Value
	{
		get;
		private set;
	}

	private void Update ()
	{
		Tank tank = TankFollowCameraRig.Instance?.MainTarget;

		if ( tank != null )
		{
			float newValue = tank.Movement.Boost / tank.Movement.maxBoost;
			if ( newValue != Value )
			{
				slider.value = newValue;
				Value = newValue;
			}
		}
	}
}
