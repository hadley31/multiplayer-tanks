using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankSpawnPoint : MonoBehaviour
{
	public Vector3 Position
	{
		get { return transform.position; }
	}

	public Quaternion Rotation
	{
		get { return transform.rotation; }
	}
}
