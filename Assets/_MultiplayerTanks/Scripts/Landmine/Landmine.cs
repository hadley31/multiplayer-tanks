using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landmine : Photon.MonoBehaviour, IProjectileInteractive, IDestroyable
{
//	public GameObject explosion;

	protected Material material;
	protected float fuseTimer;
	protected float colorSwitchTime, colorSwitchTimer;
	protected bool isColoredRed;

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

	protected void Start ()
	{
		this.fuseTimer = Fuse;
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
			Explode ();
		}
	}

	public void OnProjectileInteraction (Projectile p)
	{
		if (NetworkManager.OfflineMode)
		{
			// Destroy the landmine
			Explode ();

			// Destroy the projectile
			p.DestroyObject ();
		}
		else if (PhotonNetwork.isMasterClient)
		{
			Explode ();
			p.DestroyObjectRPC ();
		}
		else
		{
			p.DestroyObject ();
		}
	}

	private void Explode ()
	{
		if ( NetworkManager.OfflineMode )
		{
			Collider[] colliders = Physics.OverlapSphere (transform.position, Radius);
			foreach ( Collider c in colliders )
			{
				Health h = c.GetComponent<Health> ();

				if ( h != null)
				{
					h.Decrease (Damage);
					print ("Landmine damaged: " + h.gameObject.name);
				}
			}

			DestroyObject ();
		}
		else if ( PhotonNetwork.isMasterClient )
		{
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

			DestroyObjectRPC ();
		}
	}

	public void DestroyObjectRPC ()
	{
		LandmineManager.Instance.DestroyRPC (this.ID);
	}

	public void DestroyObject ()
	{
		LandmineManager.Instance.Destroy (this.ID);
	}

	private void SpawnExplosion ()
	{
		// Spawn explosion
	}

	public void SetPosition (Vector3 position)
	{
		this.transform.position = position;
	}

	public void SetFuse (float time)
	{
		this.Fuse = time;
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
}
