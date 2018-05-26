using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Health))]
public class Tank : Entity, IProjectileInteractive
{

	public Rigidbody Rigidbody
	{
		get;
		protected set;
	}

	public BoxCollider Collider
	{
		get;
		protected set;
	}

	public int Team
	{
		get;
		protected set;
	}

	protected virtual void Awake ()
	{
		this.Rigidbody = GetComponent<Rigidbody> ();
		this.Collider = GetComponent<BoxCollider> ();
	}

	public virtual void OnProjectileInteraction (Projectile p)
	{
		GetComponent<Health> ().Decrease (p.damage);
		p.DestroyObject ();
	}
}