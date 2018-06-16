using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandmineHelper : GizmoHelper
{
	public Landmine landmine;

	public override void Draw ()
	{
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere (landmine.transform.position, landmine.Radius);
	}
}
