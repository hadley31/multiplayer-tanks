using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHealth : Health
{
	public override void DecreaseRPC (int amount)
	{
		if ( NetworkManager.IsMasterClient == false)
		{
			return;
		}

		Decrease (amount);
	}

	public override void SetValueRPC (int value)
	{
		if ( NetworkManager.IsMasterClient == false )
		{
			return;
		}

		SetValue (value);
	}

	public override void SetValueToMaxRPC ()
	{
		if ( NetworkManager.IsMasterClient == false )
		{
			return;
		}

		SetValueToMax ();
	}

	public override void Destroy ()
	{
		
	}
}
