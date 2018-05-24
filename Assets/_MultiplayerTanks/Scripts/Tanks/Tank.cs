using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (EntityHealth))]
public class Tank : Entity, IProjectileInteractive
{
	public Transform top;

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

	#region Movement

	public virtual void Move (Vector3 velocity)
	{
		Rigidbody.AddForce (velocity - Rigidbody.velocity, ForceMode.VelocityChange);
	}

	public virtual void Rotate (Vector3 velocity, float turnSpeed)
	{
		if ( velocity.sqrMagnitude > Mathf.Epsilon )
		{
			Quaternion target = Quaternion.LookRotation (velocity, Vector3.up);
			Rigidbody.rotation = Quaternion.Slerp (transform.rotation, target, Time.fixedDeltaTime * turnSpeed);
		}
	}

	#endregion

	#region Top Look

	public virtual void Look (Vector3 target, float turnSpeed)
	{
		if ( target == Vector3.zero )
			return;

		Vector3 targetDirection = target - transform.position;
		targetDirection.y = 0;

		Quaternion targetRotation = Quaternion.LookRotation (targetDirection);

		top.rotation = Quaternion.Slerp (top.rotation, targetRotation, Time.deltaTime * turnSpeed);
	}

	#endregion

	#region Shoot / Landmine

	public virtual void Shoot ()
	{
		// TODO: Use a Unity Event to trigger external spawn methods

		print ("Shoot");
	}



	public virtual void LayLandmine ()
	{
		// TODO: Use a Unity Event to trigger external spawn methods

		print ("LayLandmine");
	}

	#endregion

	public virtual void OnProjectileInteraction (Projectile p)
	{
		// Use Unity Event to trigger external action
	}
}