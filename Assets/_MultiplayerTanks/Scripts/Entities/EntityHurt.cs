using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHurt : EntityBase
{
	public int damage = 0;
	public float hurtInterval = 1;

	private float m_HurtTimer = 0;
	private List<Health> m_HurtList;

	private void Update ()
	{
		if ( m_HurtList == null )
		{
			return;
		}

		m_HurtTimer += Time.deltaTime;
		if ( m_HurtTimer >= hurtInterval && m_HurtList.Count > 0 )
		{
			Hurt ();
		}
	}

	private void Hurt ()
	{
		if (NetworkManager.IsMasterClient == false)
		{
			return;
		}

		if ( m_HurtList.Count > 0 )
		{
			RemoveAllInactive ();
		}

		if ( m_HurtList.Count == 0 )
		{
			return;
		}

		for ( int i = m_HurtList.Count - 1; i >= 0; i-- )
		{
			Health h = m_HurtList[i];
			if ( h != null )
			{
				h.DecreaseRPC (damage);
			}
		}
		m_HurtTimer = 0;
	}

	public virtual void Add (Entity e)
	{
		Health h = e.Health;

		if (h == null)
		{
			return;
		}

		if ( m_HurtList == null )
		{
			m_HurtList = new List<Health> ();
		}

		if ( m_HurtList.Contains (h) == false )
		{
			m_HurtList.Add (h);
		}
	}

	public virtual void Remove (Entity e)
	{
		m_HurtList?.RemoveAll (x => x == e.Health || x == null);
	}

	public virtual void RemoveAllInactive ()
	{
		m_HurtList?.RemoveAll (x => x == null || x.gameObject.activeSelf == false || x.GetComponent<Collider> ().enabled == false || x.GetComponent<Collider> ().bounds.Intersects (Trigger.Collider.bounds) == false);
	}
}
