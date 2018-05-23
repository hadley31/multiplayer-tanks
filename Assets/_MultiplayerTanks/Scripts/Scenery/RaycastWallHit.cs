using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RaycastWallHit
{
	public static readonly RaycastWallHit Empty = new RaycastWallHit ();

	public readonly Wall wall;
	public readonly Vector3 normal;
	public readonly Vector3 point;
	public readonly Plane plane;

	public RaycastWallHit (Wall wall, Vector3 normal, Vector3 point)
	{
		this.wall = wall;
		this.normal = normal;
		this.point = point;
		this.plane = new Plane (normal, point);
	}

	public override string ToString ()
	{
		return base.ToString ();
	}

	public Vector3 GetReflectionPoint (Vector3 tankA, Vector3 tankB)
	{
		Ray rayA = new Ray (tankA, -normal);
		Ray rayB = new Ray (tankB, -normal);
		float tankADist, tankBDist;
		if (plane.Raycast (rayA, out tankADist) && plane.Raycast (rayB, out tankBDist))
		{
			Vector3 newNormal = Vector3.Cross (Vector3.up, normal);
			Plane p = new Plane (newNormal, tankB);

			if ( !p.GetSide (tankA) )
			{
				p.Flip ();
				newNormal = -newNormal;
			}

			Ray ray = new Ray (tankA, -newNormal);

			float dx;
			if (p.Raycast (ray, out dx))
			{
				float offset = tankADist / ( tankADist + tankBDist ) * dx;
				return rayA.GetPoint (tankADist) + (-newNormal * offset);
			}
		}
		return Vector3.zero;
	}

	public override bool Equals (object obj)
	{
		return obj is RaycastWallHit ? ( (RaycastWallHit) obj ).wall == this.wall : false;
	}

	public override int GetHashCode ()
	{
		return base.GetHashCode ();
	}

	public static bool operator == (RaycastWallHit a, RaycastWallHit b)
	{
		return Equals (a, b);
	}

	public static bool operator != (RaycastWallHit a, RaycastWallHit b)
	{
		return !Equals (a, b);
	}

	public static RaycastWallHit GetWallHitSphere (Vector3 origin, Vector3 direction, float radius, float distance = Mathf.Infinity)
	{
		RaycastHit hitInfo;
		if ( Physics.SphereCast (origin, radius, direction, out hitInfo, distance, LayerMask.GetMask ("Wall")) )
		{
			Wall wall = hitInfo.transform.GetComponent<Wall> ();

			if ( wall != null )
			{
				return new RaycastWallHit (wall, hitInfo.normal, hitInfo.point);
			}
		}
		return new RaycastWallHit ();
	}

	public static RaycastWallHit GetWallHitBox (Vector3 origin, Vector3 direction, Vector3 extents, Quaternion orientation, float distance, bool debug = false)
	{
		RaycastHit hitInfo;

		if (debug)
		{
			ExtDebug.DrawBoxCastBox (origin, extents, orientation, direction, distance, Color.green);
		}
		
		if ( Physics.BoxCast (origin, extents, direction, out hitInfo, orientation, distance, LayerMask.GetMask ("Wall")) )
		{
			Wall wall = hitInfo.transform.GetComponent<Wall> ();

			if ( wall != null )
			{
				return new RaycastWallHit (wall, hitInfo.normal, hitInfo.point);
			}
		}
		return new RaycastWallHit ();
	}


	public static List<RaycastWallHit> GetWallHits (Vector3 position, float raycastCount, float castRadius)
	{
		List<RaycastWallHit> hits = new List<RaycastWallHit> ();

		for ( float i = 0; i < 360; i += 360 / raycastCount )
		{
			var direction = new Vector3 (Mathf.Cos (i * Mathf.Deg2Rad), 0, Mathf.Sin (i * Mathf.Deg2Rad));
			var wallHit = GetWallHitSphere (position, direction, castRadius);

			if ( wallHit.wall != null && !hits.Exists (x => x.wall == wallHit.wall) )
			{
				hits.Add (wallHit);
			}
		}

		return hits;
	}
}
