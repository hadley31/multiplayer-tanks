using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Photon.MonoBehaviour, IProjectileInteractive, IDestroyable
{
	private const float m_interactCooldown = 0.03f;

	private float m_interactTimer;
	private float m_lifeTimer;

	public float LifeTime
	{
		get;
		private set;
	}

	public float Speed
	{
		get;
		private set;
	}

	public int Damage
	{
		get;
		private set;
	}

	public int MaxBounces
	{
		get;
		private set;
	}

	public int Bounces
	{
		get;
		private set;
	}

	public int ID
	{
		get;
		private set;
	}

	public Tank Owner
	{
		get;
		private set;
	}

	public Rigidbody Rigidbody
	{
		get;
		private set;
	}

	public Vector3 Direction
	{
		get
		{
			return Rigidbody.velocity.normalized;
		}
		private set
		{
			Rigidbody.velocity = value.normalized * Speed;
		}
	}

	#region Monobehaviors

	protected void Awake ()
	{
		Rigidbody = GetComponent<Rigidbody> ();
	}

	protected void Update ()
	{
		m_interactTimer -= Time.deltaTime;
		m_lifeTimer -= Time.deltaTime;

		if (m_lifeTimer <= 0)
		{
			DestroyObject ();
		}
	}

	protected void OnTriggerEnter (Collider col)
	{
		if (m_interactTimer > 0)
			return;

		IProjectileInteractive interaction = col.GetComponent<IProjectileInteractive> ();
		if ( interaction != null )
		{
			interaction.OnProjectileInteraction (this);
		}

		m_interactTimer = m_interactCooldown;
	}

	#endregion

	public void Bounce (Vector3 normal)
	{
		if ( normal == Vector3.zero || Vector3.Dot (normal, Direction) > 0 || Bounces <= 0 )
		{
			DestroyObject ();
			return;
		}

		Direction = Vector3.Reflect (Direction, normal);
		Bounces--;
	}

	public void OnProjectileInteraction (Projectile p)
	{
		if (NetworkManager.OfflineMode)
		{
			p.DestroyObject ();
		}
		else if (PhotonNetwork.isMasterClient)
		{
			p.DestroyObjectRPC ();
		}
		else
		{
			p.DestroyObject ();
		}
	}

	public void SetPosition (Vector3 position)
	{
		transform.position = position;
	}

	public void SetDirection (Vector3 direction)
	{
		this.Direction = direction;
	}

	public void SetLifeTime (float lifetime)
	{
		this.LifeTime = lifetime;
		this.m_lifeTimer = LifeTime;
	}

	public void SetSpeed (float speed)
	{
		this.Speed = speed;
	}

	public void SetDamage (int damage)
	{
		this.Damage = damage;
	}

	public void SetBounces (int maxBounces)
	{
		this.MaxBounces = maxBounces;

		this.Bounces = MaxBounces;
	}

	public void SetID (int id)
	{
		this.ID = id;

		SetOwner (PhotonView.Find (id & 0xFF)?.GetComponent<Tank> ());
	}

	private void SetOwner (Tank tank)
	{
		this.Owner = tank;
	}

	public void DestroyObjectRPC ()
	{
		ProjectileManager.Instance.DestroyRPC (this.ID);
	}
	
	public void DestroyObject ()
	{
		ProjectileManager.Instance.Destroy (this.ID);
	}
}
