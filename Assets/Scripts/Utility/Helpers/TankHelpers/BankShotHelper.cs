using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BankShotHelper : TankHelper
{
	public Tank target;
	public int radialRaycastCount = 4;
	public Color helperColor = Color.red;
	public Color targetColor = Color.red;

	public Transform origin
	{
		get { return target.top; }
	}

	public override void Draw ()
	{
		if ( target != null && origin != null )
		{
			var wallHits = RaycastWallHit.GetWallHits (origin.position, radialRaycastCount, 0.075f);
			foreach ( RaycastWallHit wallHit in wallHits )
			{
				var point = wallHit.GetReflectionPoint (origin.position, target.transform.position);
				if ( point != Vector3.zero && RaycastWallHit.GetWallHitSphere (origin.position, point - origin.position, 0.075f) == wallHit )
				{
					Gizmos.color = helperColor;
					Gizmos.DrawLine (origin.position, point);
				}
			}

			Gizmos.color = targetColor;
			Gizmos.DrawWireCube (target.transform.position, target.transform.localScale);
		}
        else
        {
            enabled = false;
        }
	}

	
}
