using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Photon.MonoBehaviour, IProjectileInteractive, IDestroyableRPC
{
	private const float Interaction_Cooldown = 0.01f;

	private float m_InteractTimer;
	private float m_LifeTimer;
	private float m_DistanceToNextHit;

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

	public Tank Sender
	{
		get;
		private set;
	}

	public PhotonPlayer Owner
	{
		get { return Sender.Owner; }
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
		m_InteractTimer -= Time.deltaTime;
		m_LifeTimer -= Time.deltaTime;

		if (m_LifeTimer <= 0)
		{
			DestroyObject ();
		}
	}

	protected void OnTriggerEnter (Collider col)
	{
		if (m_InteractTimer > 0)
			return;

		IProjectileInteractive interaction = col.GetComponent<IProjectileInteractive> ();
		if ( interaction != null )
		{
			interaction.OnProjectileInteraction (this);
		}

		m_InteractTimer = Interaction_Cooldown;
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
		p.DestroyObjectRPC ();
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
		this.m_LifeTimer = LifeTime;
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
	}

	public void SetSender (int viewID)
	{
		this.Sender = PhotonView.Find (viewID)?.GetComponent<Tank> ();
	}

	public void DestroyObject ()
	{
		ProjectileManager.Instance.Destroy (this.ID);
	}

	public void DestroyObjectRPC ()
	{
		ProjectileManager.Instance.DestroyRPC (this.ID);
	}
}
