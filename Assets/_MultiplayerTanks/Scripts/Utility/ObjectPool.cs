using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
	private Queue<PooledObject> reserve;

	public PooledObject prefab;
	public int reserveSize;

	public PooledObject Spawn (Vector3 position, Quaternion rotation)
	{
		if (reserve != null && reserve.Count > 0)
		{
			PooledObject obj = reserve.Dequeue ();
			obj.transform.position = position;
			obj.transform.rotation = rotation;
			return obj;
		}
		else
		{
			PooledObject obj = Instantiate (prefab, position, rotation);

			return obj;
		}
	}

	public void Reserve (PooledObject obj)
	{
		if (reserve.Count < reserveSize)
		{
			reserve.Enqueue (obj);
		}
		else
		{
			Destroy (obj.gameObject);
		}
	}
}
