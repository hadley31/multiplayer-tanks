﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandmineHealth : Health
{
	public override void DecreaseRPC (int amount)
	{
		if ( PhotonNetwork.isMasterClient == false )
		{
			return;
		}

		Decrease (amount);
	}

	public override void SetValueRPC (int value)
	{
		if ( PhotonNetwork.isMasterClient == false )
		{
			return;
		}

		SetValue (value);
	}

	public override void SetValueToMaxRPC ()
	{
		if ( PhotonNetwork.isMasterClient == false )
		{
			return;
		}

		SetValueToMax ();
	}

	public override void Destroy ()
	{

	}
}
