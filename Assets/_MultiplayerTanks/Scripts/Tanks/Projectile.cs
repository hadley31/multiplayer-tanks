using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Entity, IProjectileInteractive, IDestroyable
{
	public float speed;
	public int damage;
	public int maxBounces;
	public float interactCooldown;

	protected float interactTimer;

	public int Bounces
	{
		get;
		set;
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
		set
		{
			Rigidbody.velocity = value.normalized * speed;
		}
	}

	#region Monobehaviors

	protected void Awake ()
	{
		Rigidbody = GetComponent<Rigidbody> ();
		Bounces = maxBounces;
		interactTimer = 0;
	}

	protected void Update ()
	{
		interactTimer -= Time.deltaTime;
	}

	protected void OnTriggerEnter (Collider col)
	{
		if (interactTimer > 0)
			return;

		IProjectileInteractive interaction = col.GetComponent<IProjectileInteractive> ();
		if ( interaction != null )
		{
			interaction.OnProjectileInteraction (this);
		}

		interactTimer = interactCooldown;
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
		DestroyObject ();
	}

	public void DestroyObject ()
	{
		Destroy (gameObject);
	}
}
