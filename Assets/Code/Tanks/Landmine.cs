using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (EntityHealth))]
public class Landmine : Entity, IProjectileInteractive
{
	public const string resourceName = "Landmine";

	#region Static Spawn Methods

	public static Landmine Spawn (Landmine prefab, Vector3 position)
	{
		if (prefab)
		{
			return Instantiate (prefab, position, Quaternion.identity);
		}
		return null;
	}

	public static Landmine SpawnOnNetwork (Vector3 position, LandmineInfo info, int senderID)
	{
		if ( PhotonNetwork.inRoom )
		{
			Landmine p = PhotonNetwork.Instantiate (resourceName, position, Quaternion.identity, 0).GetComponent<Landmine> ();
			p.info = info != null ? info.Clone () : new LandmineInfo ();
			p.senderID = senderID;
			return p;
		}
		return null;
	}

	#endregion

	public Transform explosion;
	public LandmineInfo info;
	public int senderID;

	protected Material material;
	protected float fuseTimer;
	protected float colorSwitchTime, colorSwitchTimer;
	protected bool isColoredRed;

	protected void Start ()
	{
		material = GetComponent<Renderer> ().material;
		fuseTimer = info.fuseTime;
	}

	protected void Update ()
	{
		if (photonView.isMine)
		{
			fuseTimer -= Time.deltaTime;
			colorSwitchTimer -= Time.deltaTime;

			if ( colorSwitchTimer <= 0 )
			{
				isColoredRed = !isColoredRed;
				material.color = isColoredRed ? Color.red : Color.yellow;

				colorSwitchTime = fuseTimer / 10;
				colorSwitchTimer = colorSwitchTime;
			}

			if ( fuseTimer <= 0 )
			{
				DestroyObject ();
				fuseTimer = info.fuseTime;
			}
		}
		else
		{
			SerializeView ();
		}
	}

	#region Networking

	protected Vector3 networkPosition;
	protected double lastNetworkDataReceivedTime = 0;

	protected virtual void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
	{
		if ( stream.isWriting )
		{
			// This is us updating the projectile
			stream.SendNext (transform.position);
		}
		else
		{
			// This is someone elses projectile
			networkPosition = (Vector3) stream.ReceiveNext ();
			// Keep track of the timestamp for the update function
			lastNetworkDataReceivedTime = info.timestamp;
		}
	}

	protected virtual void SerializeView ()
	{

	}

	#endregion

	public void OnProjectileInteraction (Projectile p)
	{
		if (PhotonNetwork.isMasterClient)
		{
			DestroyObject ();
			p.GetComponent<EntityHealth> ().Set (0);
		}
	}

	public void DestroyObject ()
	{
		if ( photonView.isMine )
		{
			EntityHealth ourHealth = GetComponent<EntityHealth> ();
			Collider[] colliders = Physics.OverlapSphere (transform.position, info.radius);
			foreach (Collider c in colliders)
			{
				EntityHealth h = c.GetComponent<EntityHealth> ();

				if (h != null && h != ourHealth)
				{
					print (h.gameObject.name);
					h.Decrease (info.damage);
				}
			}

			PhotonNetwork.Destroy (gameObject);
		}
	}
}
