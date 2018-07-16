using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Collider))]
public class Wall : MonoBehaviour, IProjectileInteractive
{
	public static int Layer_Mask
	{
		get;
		private set;
	}

	protected virtual void Awake ()
	{
		if (Layer_Mask == 0)
		{
			Layer_Mask = LayerMask.GetMask ("Wall");
		}	
	}

	public virtual void OnProjectileInteraction (Projectile p)
	{
		Vector3 origin = p.Rigidbody.position - p.Direction * p.Speed * Time.fixedDeltaTime * 2;
		float distance = p.Speed * Time.fixedDeltaTime * 5;

		RaycastHit hitInfo;
		if ( Physics.SphereCast (origin, p.Radius, p.Direction, out hitInfo, distance, Layer_Mask) == false )
		{
			return;
		}

		p.Bounce (hitInfo.normal);
	}
}
