using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(ProjectileHealth))]
public class Projectile : Photon.MonoBehaviour, IProjectileInteractive
{
	public GameObject GameObject
	{
		get { return gameObject; }
	}

	public const float Radius = 0.075f;
	private const float Interaction_Cooldown = 0.01f;

	#region Private Fields

	private float m_InteractTimer;
	private float m_LifeTimer;
	private float m_SqrDistanceToNextHit;
	private ProjectileHealth m_Health;

	#endregion

	#region Properties

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

	public int P_ID
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

	public Tank Sender
	{
		get;
		private set;
	}

	public PhotonPlayer Owner
	{
		get { return Sender?.Owner; }
	}

	public Rigidbody Rigidbody
	{
		get;
		private set;
	}

	public ProjectileHealth Health
	{
		get;
		private set;
	}

	public bool HasBounced
	{
		get { return Bounces < MaxBounces; }
	}

	#endregion

	#region Monobehaviors

	protected void Awake ()
	{
		this.Rigidbody = GetComponent<Rigidbody> ();
		this.Health = GetComponent<ProjectileHealth> ();
	}

	protected void Update ()
	{
		m_InteractTimer -= Time.deltaTime;
		m_LifeTimer -= Time.deltaTime;

		if (m_LifeTimer <= 0)
		{
			Destroy ();
		}
	}

	protected void OnTriggerEnter (Collider col)
	{
		if (m_InteractTimer > 0)
			return;

		IProjectileInteractive interaction = col.GetComponent<IProjectileInteractive> ();
		if ( interaction != null )
		{
			OnInteraction (interaction.GameObject);
			interaction.OnProjectileInteraction (this);
		}

		m_InteractTimer = Interaction_Cooldown;
	}

	#endregion

	private void OnInteraction (GameObject obj)
	{

	}

	public void OnProjectileInteraction (Projectile p)
	{
		DestroyRPC ();
	}

	public void Bounce (Vector3 normal)
	{
		if ( normal == Vector3.zero || Vector3.Dot (normal, Direction) > 0 || Bounces <= 0 )
		{
			DestroyRPC ();
			return;
		}

		Direction = Vector3.Reflect (Direction, normal);
		Bounces--;
	}

	#region Property Setters

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
		this.P_ID = id;
	}

	public void SetSender (int viewID)
	{
		this.Sender = Tank.All.Find (x => x.ID == viewID);

		if (this.Sender?.Team != null)
		{
			SetColor (this.Sender.Team.Color);
		}
	}

	public void SetColor (Color color)
	{
		GetComponentInChildren<Renderer> ().material.color = color;
	}

	#endregion

	#region Destroy

	public void Destroy ()
	{
		ProjectileManager.Instance.Destroy (this.P_ID);
	}

	public void DestroyRPC ()
	{
		ProjectileManager.Instance.DestroyRPC (this.P_ID);
	}

	#endregion
}
