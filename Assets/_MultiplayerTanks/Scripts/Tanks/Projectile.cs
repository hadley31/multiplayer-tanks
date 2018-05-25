using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Entity, IProjectileInteractive, IDestroyable
{
	public float speed;
	public int damage;
	public int maxBounces;

	public int Bounces
	{
		get;
		set;
	}

	public Vector3 Direction
	{
		get;
		set;
	}

	#region Monobehaviors

	protected void Awake ()
	{
		Bounces = maxBounces;
	}

	protected void Update ()
	{
		Move ();
	}

	protected void OnTriggerEnter (Collider col)
	{
		IProjectileInteractive interaction = col.GetComponent<IProjectileInteractive> ();
		if ( interaction != null )
		{
			interaction.OnProjectileInteraction (this);
		}
	}

	#endregion
	

	protected virtual void Move ()
	{
		transform.Translate (Direction * Time.deltaTime * speed, Space.World);
	}

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
