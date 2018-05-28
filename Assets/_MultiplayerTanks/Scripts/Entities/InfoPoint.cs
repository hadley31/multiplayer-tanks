using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoPoint : Photon.MonoBehaviour
{
	public Vector3 point
	{
		get { return transform.position; }
	}
}
