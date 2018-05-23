using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHealth : Photon.MonoBehaviour
{
	[SerializeField]
	protected int maxHealth, _health;

	[SerializeField]
	protected bool destroyOnDie = true;

	public int Value
	{
		get
		{
			return _health;
		}
		private set
		{
			_health = Mathf.Clamp (value, 0, maxHealth);
			if ( _health <= 0 )
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
		if ( PhotonNetwork.inRoom )
		{
			photonView.RPC ("SetRPC", PhotonTargets.All, value);
		}
		else
		{
			SetRPC (value);
		}
	}

	public void SetToMax ()
	{
		Set (maxHealth);
	}

	private void Die ()
	{
		if ( PhotonNetwork.inRoom && photonView.isMine )
		{
			SendMessage ("DestroyObject", SendMessageOptions.DontRequireReceiver);
		}
		if ( destroyOnDie )
		{
			if ( PhotonNetwork.inRoom )
				PhotonNetwork.Destroy (gameObject);
			else
				Destroy (gameObject);
		}
	}

	[PunRPC]
	private void SetRPC (int value)
	{
		Value = value;
	}
}