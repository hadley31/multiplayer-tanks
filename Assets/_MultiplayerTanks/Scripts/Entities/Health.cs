using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent (typeof (Entity))]
public class Health : Photon.MonoBehaviour, IDestroyable
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

	protected virtual void Awake ()
	{
		SetToMaxHealth ();
	}

	[PunRPC]
	public void SetHealth (int value)
	{
		Value = value;
	}

	public void SetHealthRPC (int value)
	{
		photonView.RPC ("SetHealth", PhotonTargets.AllBuffered, value);
	}

	[PunRPC]
	public void DecreaseHealth (int amount)
	{
		SetHealth (Value - amount);
	}

	public void DecreaseHealthRPC (int amount)
	{
		photonView.RPC ("DecreaseHealth", PhotonTargets.AllBuffered, amount);
	}

	[PunRPC]
	public void SetToMaxHealth ()
	{
		SetHealth (maxHealth);
	}

	public void SetToMaxHealthRPC ()
	{
		photonView.RPC ("SetToMaxHealth", PhotonTargets.AllBuffered);
	}

	private void Die ()
	{
		// This may look repetative but it is important to keep the logic separate 
		// in case we decide to add more functionality later
		if (NetworkManager.OfflineMode)
		{
			onDie.Invoke ();
		}
		else if (photonView.isMine)
		{
			onDie.Invoke ();
		}
	}

	public void DestroyObject ()
	{
		Destroy (gameObject);
	}
}