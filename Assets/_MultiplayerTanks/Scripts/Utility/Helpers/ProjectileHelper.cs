using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHelper : GizmoHelper
{
	private Projectile projectile;

	private void Awake ()
	{
		projectile = GetComponent<Projectile> ();

		if (projectile == null)
		{
			Debug.LogWarning ("There is no projectile on this object!");
			this.enabled = false;
		}
	}

	public override void Draw ()
	{
		if (projectile == null)
		{
			return;
		}

		Debug.DrawRay (projectile.transform.position, projectile.Direction, Color.green);
	}
}
