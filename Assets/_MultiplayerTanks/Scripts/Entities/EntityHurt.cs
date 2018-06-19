using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHurt : MonoBehaviour
{
	public int damage = 0;
	public float hurtInterval = 1;

	private float m_HurtTimer = 0;
	private List<Health> m_HurtList;
	private int m_LastExecutedFrame = 0;

	public void UpdateHurt ()
	{
		if ( m_LastExecutedFrame == Time.frameCount )
		{
			return;
		}

		m_LastExecutedFrame = Time.frameCount;

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
}
