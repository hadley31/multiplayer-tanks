using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Health))]
public class Tank : Entity, IProjectileInteractive
{
	public int Team
	{
		get;
		protected set;
	}

	public PhotonPlayer Owner
	{
		get { return photonView.owner; }
	}

	public string OwnerAlias
	{
		get { return Owner?.NickName; }
	}

	public virtual void OnProjectileInteraction (Projectile p)
	{
		if (NetworkManager.OfflineMode)
		{
			GetComponent<Health> ().DecreaseHealth (p.Damage);
			p.DestroyObject ();
		}
		else if (PhotonNetwork.isMasterClient)
		{
			photonView.RPC ("DecreaseHealth", PhotonTargets.All, p.Damage);
			p.DestroyObjectRPC ();
		}
	}

	public void DestroyTank ()
	{
		// Temporary... Eventually we want to just disable visuals so that some components still work
		if ( NetworkManager.OfflineMode )
		{
			PhotonNetwork.Destroy (photonView);
		}
		else if ( photonView.isMine )
		{
			PhotonNetwork.Destroy (photonView);
		}
	}
}