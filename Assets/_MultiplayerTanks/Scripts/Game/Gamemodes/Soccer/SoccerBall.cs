using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody), typeof(Collider))]
public class SoccerBall : Photon.MonoBehaviour, IProjectileInteractive
{
	public float projectileHitForce = 100;
	public float tankHitForce = 300;

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
	}

	public void ResetPosition ()
	{
		SetPosition (Vector3.up * 2);
	}

	public void SetPosition (Vector3 position)
	{
		if (PhotonNetwork.isMasterClient == false)
		{
			return;
		}

		Rigidbody.MovePosition (position);
	}

	public void OnCollisionEnter (Collision collision)
	{
		Tank tank = collision.collider.GetComponent<Tank> ();

		if (tank == null)
		{
			return;
		}

		Vector3 force = ( Rigidbody.position - tank.transform.position ) * tankHitForce;

		AddForce (tank.ID, force, tank.transform.position);
	}

	public void AddForce (int id, Vector3 force, Vector3 position)
	{
		if (PhotonNetwork.isMasterClient)
		{
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
		if (PhotonNetwork.isMasterClient)
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
