﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (EntityHealth))]
public class Landmine : Entity, IProjectileInteractive
{
	public const string resourceName = "Landmine";

	#region Static Spawn Methods

	#endregion

	public Transform explosion;
	public int senderID;
	public float fuseTime;
	public float radius;
	public int damage;

	protected Material material;
	protected float fuseTimer;
	protected float colorSwitchTime, colorSwitchTimer;
	protected bool isColoredRed;

	protected void Start ()
	{
		this.material = GetComponent<Renderer> ().material;
	}

	protected void Update ()
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
			fuseTimer = this.fuseTime;
		}
	}

	#region Networking

	[PunRPC]
	public void NetworkPrime (Vector3 position, float fuse, int sender, double time)
	{
		float dt = (float) ( PhotonNetwork.time - time );
		this.transform.position = position;
		this.fuseTime = fuse;
		this.fuseTimer -= dt;
		this.senderID = sender;
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
		//EntityHealth ourHealth = GetComponent<EntityHealth> ();
		//Collider[] colliders = Physics.OverlapSphere (transform.position, radius);
		//foreach (Collider c in colliders)
		//{
		//	EntityHealth h = c.GetComponent<EntityHealth> ();

		//	if (h != null && h != ourHealth)
		//	{
		//		print (h.gameObject.name);
		//		h.Decrease (damage);
		//	}
		//}

		//PhotonNetwork.Destroy (gameObject);
	}
}