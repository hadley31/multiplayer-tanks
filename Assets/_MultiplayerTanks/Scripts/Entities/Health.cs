using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent (typeof (Entity))]
public class Health : Photon.MonoBehaviour
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
			onHealthChanged.Invoke ();
			if ( m_health <= 0 )
			{
				Die ();
			}
		}
	}

	[PunRPC]
	public void Set (int value)
	{
		Value = value;
	}

	public void SetRPC (int value)
	{
		photonView.RPC ("Set", PhotonTargets.AllBuffered, value);
	}

	[PunRPC]
	public void Decrease (int amount)
	{
		Set (Value - amount);
	}

	public void DecreaseRPC (int amount)
	{
		if ( PhotonNetwork.isMasterClient == false )
		{
			return;
		}

		SetRPC (Value - amount);
	}

	[PunRPC]
	public void SetToMax ()
	{
		Set (maxHealth);
	}

	public void SetToMaxRPC ()
	{
		if ( PhotonNetwork.isMasterClient == false )
		{
			return;
		}

		photonView.RPC ("SetToMax", PhotonTargets.AllBuffered);
	}

	private void Die ()
	{
		onDie.Invoke ();
	}

	public void DestroyObject ()
	{
		Destroy (gameObject);
	}
}