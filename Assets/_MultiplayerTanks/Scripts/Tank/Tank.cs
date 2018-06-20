using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent (typeof (Health))]
public class Tank : TankBase, IProjectileInteractive
{
	public static Tank Local
	{
		get;
		private set;
	}

	public UnityEvent onSpawn;
	public UnityEvent onDestroy;

	#region Properties

	public bool IsAlive
	{
		get;
		private set;
	}

	public bool IsPlayer
	{
		get { return TankInput != null; }
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
		if (photonView.isMine && this.IsPlayer)
		{
			Local = this;
			TankFollowCameraRig.Instance.OnlyFollow (this);
		}
	}

	private void OnDestroy ()
	{
		if (this == Local)
		{
			Local = null;
		}

		TankFollowCameraRig.Instance?.StopFollowing (this);
	}

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

		p.DestroyRPC ();
	}

	#region Spawn / Destroy

	[PunRPC]
	public void Spawn ()
	{
		IsAlive = true;

		onSpawn.Invoke ();
	}

	public void SpawnRPC ()
	{
		if ( photonView.isMine == false )
		{
			return;
		}

		photonView.RPC ("Spawn", PhotonTargets.All);
	}

	public void Respawn (int delay = 0)
	{
		if ( photonView.isMine == false || IsAlive == true )
		{
			return;
		}

		Invoke ("SpawnRPC", delay);
	}

	[PunRPC]
	public void Destroy ()
	{
		IsAlive = false;

		onDestroy.Invoke ();
	}

	public void DestroyRPC ()
	{
		if ( photonView.isMine == false )
		{
			return;
		}

		photonView.RPC ("Destroy", PhotonTargets.All);
	}

	#endregion
}