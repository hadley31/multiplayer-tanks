using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateObject : MonoBehaviour
{
	public GameObject obj;

	[Header ("Position")]
	public Transform parent;
	public Vector3 position;
	public Vector3 rotation;

	public GameObject Spawn ()
	{
		if (obj == null)
		{
			return null;
		}

		return Instantiate (obj, position, Quaternion.Euler (rotation), parent);
	}

	public T Spawn<T> ()
	{
		return Spawn ().GetComponent<T> ();
	}
}
