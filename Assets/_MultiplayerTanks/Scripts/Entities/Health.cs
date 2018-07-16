using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent (typeof (Entity))]
public class Health : EntityBase
{
	[SerializeField]
	private int m_MaxHealth;

	[SerializeField]
	private bool m_GodMode;
	
	public IntUnityEvent onHealthChanged;
	public UnityEvent onDie;

	protected int m_health;

	public virtual int Max
	{
		get { return m_MaxHealth; }
		set { m_MaxHealth = value; }
	}

	public virtual int Value
	{
		get
		{
			return m_health;
		}
		private set
		{
			m_health = Mathf.Clamp (value, 0, m_MaxHealth);
			onHealthChanged.Invoke (m_health);
			if ( m_health <= 0 )
			{
				Die ();
			}
		}
	}

	public virtual bool GodMode
	{
		get { return m_GodMode; }
		private set { m_GodMode = value; }
	}

	protected virtual void Start ()
	{
		SetValueToMax ();
	}

	[PunRPC]
	public virtual void SetValue (int value)
	{
		Value = value;
	}

	public virtual void SetValueRPC (int value)
	{
		if ( PhotonNetwork.isMasterClient == false )
		{
			return;
		}

		photonView.RPC ("SetValue", PhotonTargets.AllBuffered, value);
	}

	[PunRPC]
	public virtual void Decrease (int amount)
	{
		SetValue (Value - amount);
	}

	public virtual void DecreaseRPC (int amount)
	{
		if ( PhotonNetwork.isMasterClient == false )
		{
			return;
		}

		SetValueRPC (Value - amount);
	}

	[PunRPC]
	public virtual void SetValueToMax ()
	{
		SetValue (m_MaxHealth);
	}

	public virtual void SetValueToMaxRPC ()
	{
		if ( PhotonNetwork.isMasterClient == false )
		{
			return;
		}

		photonView.RPC ("SetValueToMax", PhotonTargets.AllBuffered);
	}

	[PunRPC]
	public virtual void SetMaxValue (int maxValue, bool setValueToMax = false)
	{
		Max = maxValue;

		if (setValueToMax)
		{
			Value = Max;
		}
	}

	public virtual void SetMaxValueRPC (int maxValue, bool setValueToMax = false)
	{
		if ( PhotonNetwork.isMasterClient == false )
		{
			return;
		}

		photonView.RPC ("SetMaxValue", PhotonTargets.All, maxValue, setValueToMax);
	}

	protected virtual void Die ()
	{
		onDie.Invoke ();
	}

	public virtual void Destroy ()
	{
		Destroy (gameObject);
	}
}