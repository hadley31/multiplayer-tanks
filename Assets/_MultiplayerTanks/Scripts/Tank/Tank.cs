using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent (typeof (Health))]
public class Tank : TankBase, IProjectileInteractive
{
	public static Color Default_Color
	{
		get { return new Color (0.85f, 0.85f, 0.85f); }
	}

	public static Tank Local
	{
		get;
		private set;
	}
	public static readonly List<Tank> All = new List<Tank> ();
	public static readonly List<Tank> AllAlive = new List<Tank> ();
	public static readonly List<Tank> AllDead = new List<Tank> ();

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

	public Team Team
	{
		get { return Owner.GetTeam (); }
		set { Owner.SetTeam (value); }
	}

	public PhotonPlayer Owner
	{
		get { return photonView.owner; }
	}

	public int ID
	{
		get { return photonView.viewID; }
	}

	public string OwnerAlias
	{
		get { return Owner?.NickName ?? string.Empty; }
	}

	public int LastHitID
	{
		get;
		set;
	}

	#endregion

	#region Monobehaviours

	private void Awake ()
	{
		if ( photonView.isMine && this.IsPlayer )
		{
			Local = this;
		}

		if ( All.Contains (this) == false )
		{
			All.Add (this);
		}
	}

	private void Start ()
	{
		if ( photonView.isMine && this.IsPlayer )
		{
			print ("only follow");
			TankFollowCameraRig.Instance.OnlyFollow (this);
		}

		Spawn ();
	}

	private void OnDestroy ()
	{
		if (this == Local)
		{
			Local = null;
		}

		All.Remove (this);
		AllAlive.Remove (this);
		AllDead.Remove (this);

		TankFollowCameraRig.Instance?.StopFollowing (this);
	}

	#endregion

	public virtual void OnProjectileInteraction (Projectile p)
	{
		if ( p.Sender.ID == this.ID && p.HasBounced == false )
		{
			return;
		}

		if ( PhotonNetwork.isMasterClient )
		{

			Health.DecreaseRPC (p.Damage);
			LastHitID = p.Sender.ID;
		}

		p.DestroyRPC ();
	}

	#region Spawn / Destroy

	[PunRPC]
	public void Spawn ()
	{
		IsAlive = true;

		UpdateList ();

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

		Invoke ("SpawnRPC", 3);
	}

	[PunRPC]
	public void Destroy ()
	{
		IsAlive = false;

		UpdateList ();

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

	private void UpdateList ()
	{
		if (IsAlive)
		{
			AllDead.Remove (this);

			if (AllAlive.Contains (this) == false)
			{
				AllAlive.Add (this);
			}
		}
		else
		{
			AllAlive.Remove (this);

			if ( AllDead.Contains (this) == false )
			{
				AllDead.Add (this);
			}
		}
	}
}