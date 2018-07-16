using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class AimHelper : GizmoHelper
{
	public int reflectCount;
	public Color helperColor = Color.green;

	private TankShoot m_Shoot;

	private void Awake ()
	{
		m_Shoot = GetComponent<TankShoot> ();
	}

	public override void Draw ()
	{
		Gizmos.color = helperColor;
		if ( m_Shoot?.spawnPoint != null && m_Shoot.Tank.IsAlive )
		{
			List<Vector3> points = new List<Vector3> ();
			Vector3 position = m_Shoot.spawnPoint.position;
			Vector3 direction = m_Shoot.spawnPoint.forward;

			points.Add (position);
			for ( int i = 0; i <= m_Shoot.bounces; i++ )
			{
				RaycastHit hitInfo;
				if ( Physics.SphereCast (position, m_Shoot.radius, direction, out hitInfo, 1000) )
				{
					IProjectileInteractive interactive = hitInfo.transform.GetComponent<IProjectileInteractive> ();

					if ( interactive == null )
					{
						break;
					}

					points.Add (hitInfo.point);

					if ( hitInfo.transform.GetComponent<Wall> () == null )
					{
						break;
					}

					position = hitInfo.point + hitInfo.normal * m_Shoot.radius;
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
	}
}
