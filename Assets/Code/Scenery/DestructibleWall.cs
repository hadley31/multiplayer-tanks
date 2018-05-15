using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleWall : Wall, IDestroyable
{
	public void DestroyObject ()
	{
		PhotonNetwork.Destroy (gameObject);
	}
}
