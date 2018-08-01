using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody), typeof(Collider))]
public class SoccerBall : Photon.MonoBehaviour, IProjectileInteractive
{
	public float projectileHitForce = 100;
	public float tankHitForce = 300;

	public GameObject GameObject
	{
		get { return gameObject; }
	}

	public Rigidbody Rigidbody
	{
		get;
		private set;
	}

	public Collider Collider
	{
		get;
		private set;
	}

	public int LastViewToTouch
	{
		get;
		private set;
	}

	private void Awake ()
	{
		Rigidbody = GetComponent<Rigidbody> ();
		Collider = GetComponent<Collider> ();

		Rigidbody.useGravity = NetworkManager.IsMasterClient;
	}

	public void OnMasterClientSwitched (PhotonPlayer newMasterClient)
	{
		Rigidbody.useGravity = NetworkManager.IsMasterClient;
	}

	public void ResetPosition ()
	{
		if ( NetworkManager.IsMasterClient == false )
		{
			return;
		}

		SetPositionRPC (Vector3.up * 2);
	}

	public void SetPositionRPC (Vector3 position)
	{
		if (PhotonNetwork.isMasterClient == false)
		{
			return;
		}

		photonView.RPC ("SetPosition", PhotonTargets.All, position);
	}

	[PunRPC]
	public void SetPosition (Vector3 position)
	{
		Rigidbody.MovePosition (position);
	}

	private void OnTriggerEnter (Collider other)
	{
		Tank tank = other.GetComponent<Tank> ();

		if (tank == null)
		{
			return;
		}

		Vector3 force = ( Rigidbody.position - tank.transform.position ) * tankHitForce;

		AddForce (tank.ID, force, tank.transform.position);
	}

	private void Update ()
	{
		if (NetworkManager.IsMasterClient == false)
		{
			return;
		}

		if (Input.GetKeyDown (KeyCode.Backspace))
		{
			ResetPosition ();
		}
	}

	public void AddForce (int id, Vector3 force, Vector3 position)
	{
		if (NetworkManager.IsMasterClient)
		{
			Rigidbody.useGravity = true;
			AddForceRPC (id, force, position);
			return;
		}
		else
		{
			Rigidbody.AddForceAtPosition (force, position, ForceMode.Acceleration);
			photonView.RPC ("AddForceRPC", PhotonTargets.MasterClient, id, force, position);
		}
		
	}

	[PunRPC]
	private void AddForceRPC (int id, Vector3 force, Vector3 position)
	{
		if (NetworkManager.IsMasterClient)
		{
			Rigidbody.AddForceAtPosition (force, position, ForceMode.Acceleration);
			LastViewToTouch = id;
		}
	}

	public void OnProjectileInteraction (Projectile p)
	{
		if ( p.Sender.ID == Tank.Local.ID )
		{
			AddForce (p.Sender.ID, p.Direction * projectileHitForce, p.Rigidbody.position);
		}

		p.DestroyRPC ();
	}
}
