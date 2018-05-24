using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class AimHelper : TankHelper
{
	public int reflectCount;
	public float castRadius;
	public Color helperColor = Color.green;

	public override void Draw ()
	{
		Gizmos.color = helperColor;
		if ( tank != null )
		{
			List<Vector3> points = new List<Vector3> ();
			Vector3 position = tank.transform.position;
			Vector3 direction = tank.transform.forward;

			points.Add (position);
			for ( int i = 0; i <= reflectCount; i++ )
			{
				RaycastHit hitInfo;
				if ( Physics.SphereCast (position, castRadius, direction, out hitInfo, 1000, LayerMask.GetMask ("Wall", "Tank")) )
				{
					points.Add (hitInfo.point);

					if ( hitInfo.transform.GetComponent<Tank> () != null )
					{
						break;
					}

					position = hitInfo.point + hitInfo.normal * castRadius;
					direction = Vector3.Reflect (direction, hitInfo.normal);
				}
				else
				{
					break;
				}
			}
			for ( int i = 1; i < points.Count; i++ )
			{
				Gizmos.DrawLine (points[i - 1], points[i]);
			}
		}
        else
        {
            enabled = false;
        }
	}
}
