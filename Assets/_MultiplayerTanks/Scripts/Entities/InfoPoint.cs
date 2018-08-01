using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoPoint : EntityBase
{
	public Vector3 point
	{
		get { return transform.position; }
	}
}
