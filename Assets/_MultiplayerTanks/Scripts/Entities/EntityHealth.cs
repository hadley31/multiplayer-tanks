using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntityHealth : MonoBehaviour
{
	public int maxHealth;
	[Space (10)]
	public UnityEvent onHealthChanged;
	public UnityEvent onDie;


	protected int m_health;

	public int Value
	{
		get
		{
			return m_health;
		}
		private set
		{
			m_health = Mathf.Clamp (value, 0, maxHealth);
			if ( m_health <= 0 )
			{
				Die ();
			}
		}
	}

	protected virtual void Awake ()
	{
		SetToMax ();
	}

	public void Decrease (int amount)
	{
		Set (Value - amount);
	}

	public void Set (int value)
	{
		Value = value;
	}

	public void SetToMax ()
	{
		Set (maxHealth);
	}

	private void Die ()
	{
		Destroy (this);
	}
}