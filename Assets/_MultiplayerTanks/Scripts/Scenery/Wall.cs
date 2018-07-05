using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Collider))]
public class Wall : MonoBehaviour, IProjectileInteractive
{
	public GameObject GameObject
	{
		get { return gameObject; }
	}

	public Bounds Bounds
	{
		get { return Collider.bounds; }
	}

	public Collider Collider
	{
		get;
		private set;
	}

	protected virtual void Awake ()
	{
		Collider = GetComponent<Collider> ();
	}

	public virtual void OnProjectileInteraction (Projectile p)
	{
		p.Bounce (GetNormal (p.transform.position, p.Direction, 0.15f));
	}

	public Vector3 GetNormal (Vector3 position, Vector3 direction, float threshold)
	{
		Vector3 offset = position - Bounds.center;
		Vector3 normal = Vector3.zero;
		float distance = 0;
		float minDistance = float.MaxValue;

		distance = Mathf.Abs (offset.x + Bounds.extents.x);
		if ( direction.x > 0 && distance < minDistance )
		{
			normal = Vector3.left;
			minDistance = distance;
		}

		distance = Mathf.Abs (offset.x - Bounds.extents.x);
		if ( direction.x < 0 && distance < minDistance )
		{
			normal = Vector3.right;
			minDistance = distance;
		}
			
		distance = Mathf.Abs (offset.z + Bounds.extents.z);
		if ( direction.z > 0 && distance < minDistance )
		{
			normal = Vector3.back;
			minDistance = distance;
		}

		distance = Mathf.Abs (offset.z - Bounds.extents.z);
		if ( direction.z < 0 && distance < minDistance )
		{
			normal = Vector3.forward;
			minDistance = distance;
		}
			

		return normal;
	}
}
