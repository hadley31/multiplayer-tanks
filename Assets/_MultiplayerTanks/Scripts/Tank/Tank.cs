using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent (typeof (Health))]
public class Tank : TankBase, IProjectileInteractive
{
	public UnityEvent onSpawn;
	public UnityEvent onDestroy;

	#region Properties

	public bool IsAlive
	{
		get;
		private set;
	}

	public int Team
	{
		get;
		private set;
	}

	public PhotonPlayer Owner
	{
		get { return photonView.owner; }
	}

	public string OwnerAlias
	{
		get { return Owner?.NickName ?? string.Empty; }
	}

	public PhotonPlayer LastProjectileOwner
	{
		get;
		set;
	}

	#endregion

	private void Start ()
	{
		Spawn ();
	}

	public virtual void OnProjectileInteraction (Projectile p)
	{
		if ( PhotonNetwork.isMasterClient )
		{
			Health.DecreaseRPC (p.Damage);
			LastProjectileOwner = p.Owner;
		}

		p.DestroyObjectRPC ();
	}

	public void SpawnRPC ()
	{
		if ( photonView.isMine == false )
		{
			return;
		}

		photonView.RPC ("Spawn", PhotonTargets.All);
	}

	[PunRPC]
	public void Spawn ()
	{
		IsAlive = true;

		onSpawn.Invoke ();
	}

	public void DestroyRPC ()
	{
		if ( photonView.isMine == false )
		{
			return;
		}

		photonView.RPC ("Destroy", PhotonTargets.All);
	}

	[PunRPC]
	public void Destroy ()
	{
		IsAlive = false;

		onDestroy.Invoke ();

		if ( photonView.isMine )
		{
			Invoke ("SpawnRPC", 5);
		}
	}
}