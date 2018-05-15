using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(EntityHealth))]
public class Projectile : Entity, IProjectileInteractive, IDestroyable
{
	public const string resourceName = "Projectile";

	#region Static Spawn Methods

	public static Projectile Spawn (Projectile prefab, Vector3 position, Vector3 direction, ProjectileInfo info)
	{
		Projectile p = Instantiate (prefab, position, Quaternion.identity);
		p.direction = direction;
		p.info = info.Clone ();
		return p;
	}

	public static Projectile SpawnOnNetwork (Vector3 position, Vector3 direction, ProjectileInfo info, int senderID)
	{
		if (PhotonNetwork.inRoom)
		{
			Projectile p = PhotonNetwork.Instantiate (resourceName, position, Quaternion.identity, 0).GetComponent<Projectile> ();
			p.direction = direction;
			p.info = info != null ? info.Clone () : new ProjectileInfo ();
			p.senderID = senderID;
			return p;
		}
		return null;
	}

	#endregion

	public Vector3 direction;
	public ProjectileInfo info;
	public int senderID;

	#region Monobehaviors

	protected void Start ()
	{
		Destroy (gameObject, info.lifeSpan);
		transform.localScale = Vector3.one * info.radius * 2;
	}

	protected void Update ()
	{
		if (photonView.isMine)
		{
			Move ();
		}
		else
		{
			SerializeView ();
		}
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

		Vector3 newPosition = Vector3.Lerp (transform.position, estimatedPosition, info.moveSpeed * Time.deltaTime);

		if (Vector3.SqrMagnitude (estimatedPosition - transform.position) > 4f)
		{
			newPosition = estimatedPosition;
		}

		transform.position = newPosition;
	}

	#endregion

	protected virtual void Move ()
	{
		transform.Translate (direction * Time.deltaTime * info.moveSpeed, Space.World);
	}

	public void Bounce (Vector3 normal)
	{
		print ("Bounce");
		if ( info.bounces <= 0 )
		{
			DestroyObject ();
			return;
		}

		direction = Vector3.Reflect (direction, normal);
		info.bounces--;
	}

	public void OnProjectileInteraction (Projectile p)
	{
		DestroyObject ();
	}

	public void DestroyObject ()
	{
		if (photonView.isMine)
		{
			PhotonNetwork.Destroy (gameObject);
		}
	}
}
