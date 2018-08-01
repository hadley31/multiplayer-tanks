using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Health))]
public class DestructibleWall : EntityBase
{
	public void Destroy ()
	{
		if ( NetworkManager.IsMasterClient )
		{
			photonView.RPC ("Hide", PhotonTargets.AllBufferedViaServer);
		}
	}

	[PunRPC]
	private void Hide ()
	{
		gameObject.SetActive (false);
	}
}
