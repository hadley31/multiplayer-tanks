using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBase : Photon.MonoBehaviour
{
	private Entity m_Entity;

	public Entity Entity
	{
		get
		{
			if ( m_Entity == null )
			{
				m_Entity = GetComponent<Entity> ();
			}

			return m_Entity;
		}
	}

	private Health m_Health;

	public Health Health
	{
		get
		{
			if (m_Health == null)
			{
				m_Health = GetComponent<Health> ();
			}

			return m_Health;
		}
	}

	private Trigger m_Trigger;

	public Trigger Trigger
	{
		get
		{
			if (m_Trigger == null)
			{
				m_Trigger = GetComponent<Trigger> ();
			}

			return m_Trigger;
		}
	}
}
