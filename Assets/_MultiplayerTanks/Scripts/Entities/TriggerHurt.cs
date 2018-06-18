using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHurt : Trigger
{
	public int damage = 0;
	public float hurtInterval = 1;

	protected float m_HurtTimer = 0;
	protected List<Health> m_HurtList;

	protected override void OnTriggerEnterEnt (Entity ent)
	{
		Add (ent.GetComponent<Health> ());
		base.OnTriggerEnterEnt (ent);
	}

	protected override void OnTriggerStayEnt (Entity ent)
	{
		if ( m_HurtList != null && m_HurtList.Count > 0 )
		{
			m_HurtTimer += Time.deltaTime;
			if ( m_HurtTimer >= hurtInterval )
			{
				for ( int i = m_HurtList.Count - 1; i >= 0; i-- )
				{
					Health h = m_HurtList[i];
					if ( h != null )
						h.DecreaseRPC (damage);
				}
				m_HurtTimer = 0;
			}
		}
	}

	protected override void OnTriggerExitEnt (Entity ent)
	{
		Remove (ent.GetComponent<Health> ());
		base.OnTriggerEnterEnt (ent);
	}

	protected virtual void Add (Health h)
	{
		if ( h != null )
		{
			if ( m_HurtList == null )
				m_HurtList = new List<Health> ();

			if ( m_HurtList.Contains (h) == false )
			{
				m_HurtList.Add (h);
			}
		}
	}

	protected virtual void Remove (Health h)
	{
		m_HurtList?.RemoveAll (x => x == h);
	}
}
