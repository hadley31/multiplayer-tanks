using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody), typeof(Collider))]
public class SoccerBall : MonoBehaviour, IProjectileInteractive
{
	public float projectileHitForce = 100;

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
		if ( PhotonNetwork.isMasterClient == false )
		{
			return;
		}
		Tank tank = collision.collider.GetComponent<Tank> ();

		if (tank == null)
		{
			return;
		}

		LastViewToTouch = tank.photonView.viewID;
	}

	public void OnProjectileInteraction (Projectile p)
	{
		if ( PhotonNetwork.isMasterClient == true )
		{
			Rigidbody.AddForceAtPosition (p.Direction * projectileHitForce, p.Rigidbody.position, ForceMode.Acceleration);
			LastViewToTouch = p.Sender.photonView.viewID;
		}

		p.DestroyRPC ();
	}
}
