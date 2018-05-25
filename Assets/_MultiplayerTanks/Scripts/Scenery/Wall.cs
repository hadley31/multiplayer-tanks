using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour, IProjectileInteractive
{
	public Bounds Bounds
	{
		get;
		protected set;
	}

	protected virtual void Awake ()
	{
		Bounds = new Bounds (transform.position, transform.lossyScale);
	}

	public virtual void OnProjectileInteraction (Projectile p)
	{
		p.Bounce (GetNormal (p.transform.position, p.Direction, 0.15f));
	}

	public Vector3 GetNormal (Vector3 position, Vector3 direction, float threshold)
	{
		Vector3 offset = position - Bounds.center;
		
		if (direction.x > 0 && Mathf.Abs(Bounds.extents.x + offset.x) < threshold )
			return Vector3.left;
		if ( direction.x < 0 && Mathf.Abs (offset.x - Bounds.extents.x) < threshold )
			return Vector3.right;
		if ( direction.z > 0 && Mathf.Abs (offset.z + Bounds.extents.z) < threshold )
			return Vector3.back;
		if ( direction.z < 0 && Mathf.Abs (offset.z - Bounds.extents.z) < threshold )
			return Vector3.forward;
		return Vector3.zero;
	}
}
