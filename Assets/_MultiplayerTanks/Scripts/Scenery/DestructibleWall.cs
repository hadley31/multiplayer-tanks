using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Health))]
public class DestructibleWall : Wall
{
	public void Destroy ()
	{
		if (PhotonNetwork.isMasterClient)
		{
			PhotonNetwork.Destroy (gameObject);
		}
	}
}
