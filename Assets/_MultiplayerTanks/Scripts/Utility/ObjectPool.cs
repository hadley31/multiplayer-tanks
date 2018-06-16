using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
	private readonly Queue<PooledObject> reserve = new Queue<PooledObject> ();

	public PooledObject prefab;
	public int reserveSize;


	public int ReserveCount
	{
		get { return reserve.Count; }
	}

	public PooledObject Spawn ()
	{
		if ( reserve != null && reserve.Count > 0 )
		{
			PooledObject obj = reserve.Dequeue ();

			obj.gameObject.SetActive (true);

			return obj;
		}
		else
		{
			PooledObject obj = Instantiate (prefab);

			obj.Prime (this);

			return obj;
		}
	}

	public T Spawn<T> () where T : Component
	{
		return Spawn ().GetComponent<T> ();
	}

	public void Reserve (PooledObject obj)
	{
		if ( reserve.Count < reserveSize && !reserve.Contains (obj) )
		{
			obj.gameObject.SetActive (false);
			reserve.Enqueue (obj);
		}
		else
		{
			Destroy (obj.gameObject);
		}
	}

	public void Reserve (Component obj)
	{
		PooledObject pooledObj = obj.GetComponent<PooledObject> ();

		if ( pooledObj == null )
		{
			Debug.Log ("Attempted to reserve a gameobject without a PooledObject component!");
			return;
		}

		Reserve (pooledObj);
	}

	public void Reserve (GameObject obj)
	{
		PooledObject pooledObj = obj.GetComponent<PooledObject> ();

		if (pooledObj == null)
		{
			Debug.Log ("Attempted to reserve a gameobject without a PooledObject component!");
			return;
		}

		Reserve (pooledObj);
	}
}
