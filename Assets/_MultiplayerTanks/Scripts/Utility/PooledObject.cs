using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledObject : MonoBehaviour
{
	public ObjectPool Pool
	{
		get;
		private set;
	}

	public void Prime (ObjectPool pool)
	{
		this.Pool = pool;
	}

	public void DestroyObject ()
	{
		Pool.Reserve (this);
	}
}
