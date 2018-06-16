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

	private void Awake ()
	{
		Spawn ();
	}

	public virtual void OnProjectileInteraction (Projectile p)
	{
		if ( PhotonNetwork.isMasterClient )
		{
			Health.DecreaseHealthRPC (p.Damage);


			LastProjectileOwner = p.Owner;
		}

		p.DestroyObjectRPC ();
	}

	public void Spawn ()
	{
		IsAlive = true;

		onSpawn.Invoke ();
	}

	public void Destroy ()
	{
		IsAlive = false;

		if ( photonView.isMine )
		{
			Invoke ("Spawn", 5);
		}

		onDestroy.Invoke ();
	}
}