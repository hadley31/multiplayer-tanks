using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(EntityHealth))]
public class Projectile : Entity, IProjectileInteractive, IDestroyable
{
	public const string resourceName = "Projectile";

	#region Static Spawn Methods

	public static void Spawn (Vector3 position, Vector3 direction, float speed, int bounces, int sender)
	{
		Projectile p = PhotonNetwork.Instantiate (resourceName, position, Quaternion.identity, 0).GetComponent<Projectile> ();

		p.direction = direction;
		p.senderID = sender;
		p.speed = speed;
		p.bounces = bounces;

		if (!PhotonNetwork.isMasterClient)
		{
			p.photonView.TransferOwnership (PhotonNetwork.masterClient);
		}

		p.photonView.RPC ("NetworkPrime", PhotonTargets.Others, position, direction, speed, bounces, sender, PhotonNetwork.time);
	}

	#endregion

	public Vector3 direction;
	public float speed;
	public int senderID;
	public int bounces;

	#region Monobehaviors

	protected void Update ()
	{
		Move ();
	}

	protected void OnTriggerEnter (Collider col)
	{
		IProjectileInteractive interaction = col.GetComponent<IProjectileInteractive> ();
		if ( interaction != null )
		{
			interaction.OnProjectileInteraction (this);
		}
	}

	#endregion

	#region Networking

	protected Vector3 networkPosition;
	protected Vector3 networkDirection;
	protected double lastNetworkDataReceivedTime = 0;

	protected virtual void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
	{
		if ( stream.isWriting )
		{
			// This is us updating the projectile
			stream.SendNext (transform.position);
			stream.SendNext (direction);
		}
		else
		{
			// This is someone elses projectile
			networkPosition = (Vector3) stream.ReceiveNext ();
			networkDirection = (Vector3) stream.ReceiveNext ();

			// Keep track of the timestamp for the update function
			lastNetworkDataReceivedTime = info.timestamp;
		}
	}

	protected virtual void SerializeView ()
	{
		float pingInSeconds = PhotonNetwork.GetPing () * 0.001f;
		float timeSinceLastUpdate = (float) ( PhotonNetwork.time - lastNetworkDataReceivedTime );

		float totalTimePassed = pingInSeconds + timeSinceLastUpdate;

		Vector3 estimatedPosition = networkPosition + ( networkDirection * totalTimePassed );

		Vector3 newPosition = Vector3.Lerp (transform.position, estimatedPosition, speed * Time.deltaTime);

		if (Vector3.SqrMagnitude (estimatedPosition - transform.position) > 4f)
		{
			newPosition = estimatedPosition;
		}

		transform.position = newPosition;
	}


	[PunRPC]
	public void NetworkPrime (Vector3 position, Vector3 direction, float speed, int bounces, int sender, double time)
	{
		float dt = (float)(PhotonNetwork.time - time);
		transform.position = position + direction * dt * speed;
		this.direction = direction;
		this.speed = speed;
		this.bounces = bounces;
		this.senderID = sender;
	}

	#endregion

	protected virtual void Move ()
	{
		transform.Translate (direction * Time.deltaTime * speed, Space.World);
	}

	public void Bounce (Vector3 normal)
	{
		if ( normal == Vector3.zero || Vector3.Dot (normal, direction) > 0 || bounces <= 0 )
		{
			DestroyObject ();
			return;
		}

		direction = Vector3.Reflect (direction, normal);
		bounces--;
	}

	public void OnProjectileInteraction (Projectile p)
	{
		DestroyObject ();
	}

	public void DestroyObject ()
	{
		if (PhotonNetwork.inRoom)
		{
			if (PhotonNetwork.isMasterClient)
			{
				PhotonNetwork.Destroy (gameObject);
			}
		}
		else
		{
			Destroy (gameObject);
		}
	}
}
