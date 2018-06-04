using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void EntityDelegate (Entity e);

public class Entity : Photon.MonoBehaviour
{
	private Health m_Health;

	public Health Health
	{
		get
		{
			if (!m_Health)
			{
				m_Health = GetComponent<Health> ();
			}

			return m_Health;
		}
	}

	public bool Is<T> ()
	{
		return GetComponent<T> () != null;
	}
}
