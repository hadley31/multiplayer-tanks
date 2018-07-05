﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (LandmineHealth))]
public class Landmine : Photon.MonoBehaviour, IProjectileInteractive
{
	// public GameObject explosion;

	#region Private Fields

	private Material m_Material;
	private float m_FuseTimer;
	private float m_ColorSwitchTime, m_ColorSwitchTimer;
	private bool m_IsColoredRed;
	private bool m_HasExploded;

	#endregion

	#region Properties

	public GameObject GameObject
	{
		get { return gameObject; }
	}

	public int Damage
	{
		get;
		private set;
	}

	public float Radius
	{
		get;
		private set;
	}

	public float Fuse
	{
		get;
		private set;
	}

	public int ID
	{
		get;
		private set;
	}

	public Tank Sender
	{
		get;
		private set;
	}

	public PhotonPlayer Owner
	{
		get { return Sender.Owner; }
	}

	public LandmineHealth Health
	{
		get;
		private set;
	}

	#endregion

	#region Monobehaviours

	private void Awake ()
	{
		this.m_Material = GetComponentInChildren<Renderer> ().material;
		this.Health = GetComponent<LandmineHealth> ();
	}

	private void Update ()
	{
		m_FuseTimer -= Time.deltaTime;
		m_ColorSwitchTimer -= Time.deltaTime;

		if ( m_ColorSwitchTimer <= 0 )
		{
			m_IsColoredRed = !m_IsColoredRed;
			m_Material.color = m_IsColoredRed ? Color.red : Color.yellow;

			m_ColorSwitchTime = m_FuseTimer / 10;
			m_ColorSwitchTimer = m_ColorSwitchTime;
		}

		if ( m_FuseTimer <= 0 )
		{
			Explode ();
		}
	}

	#endregion

	public void OnProjectileInteraction (Projectile p)
	{
		if (PhotonNetwork.isMasterClient)
		{
			Explode ();	
		}
	}

	public void Explode ()
	{
		if ( PhotonNetwork.isMasterClient == false )
		{
			return;
		}

		if ( m_HasExploded == true )
		{
			return;
		}

		m_HasExploded = true;

		Collider[] colliders = Physics.OverlapSphere (transform.position, Radius);
		foreach ( Collider c in colliders )
		{
			Health h = c.GetComponent<Health> ();

			if ( h != null )
			{
				h.DecreaseRPC (Damage);
				print ("Landmine damaged: " + h.gameObject.name);
			}
		}

		DestroyRPC ();
	}

	private void SpawnExplosion ()
	{
		// Spawn explosion
	}

	#region Property Setters

	public void SetPosition (Vector3 position)
	{
		this.transform.position = position;
	}

	public void SetFuse (float time)
	{
		this.Fuse = time;

		this.m_FuseTimer = Fuse;
		this.m_HasExploded = false;
	}

	public void SetDamage (int damage)
	{
		this.Damage = damage;
	}

	public void SetRadius (float radius)
	{
		this.Radius = radius;
	}

	public void SetID (int id)
	{
		this.ID = id;
	}

	public void SetSender (int viewID)
	{
		this.Sender = PhotonView.Find (viewID)?.GetComponent<Tank> ();
	}

	#endregion

	#region Destroy

	public void Destroy ()
	{
		LandmineManager.Instance.Destroy (this.ID);
	}

	public void DestroyRPC ()
	{
		LandmineManager.Instance.DestroyRPC (this.ID);
	}

	#endregion
}
