using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
	private Queue<PooledObject> reserve = new Queue<PooledObject> ();

	public PooledObject prefab;
	public int reserveSize;


	public T Spawn<T> () where T : Component
	{
		if ( reserve != null && reserve.Count > 0 )
		{
			PooledObject obj = reserve.Dequeue ();

			obj.gameObject.SetActive (true);

			return obj.GetComponent<T> ();
		}
		else
		{
			PooledObject obj = Instantiate (prefab);

			obj.Prime (this);

			return obj.GetComponent<T> ();
		}
	}

	public void Reserve (PooledObject obj)
	{
		Debug.Log ("ObjectPool::Reserve()");
		Debug.Log ("Reserves: " + reserve.Count);

		if (reserve.Count < reserveSize)
		{
			reserve.Enqueue (obj);
			obj.gameObject.SetActive (false);
		}
		else
		{
			Destroy (obj.gameObject);
		}
	}
}
