using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void EntityDelegate (Entity e);

[System.Serializable]
public class Entity : EntityBase
{
	public bool Is<T> ()
	{
		return GetComponent<T> () != null;
	}
}
