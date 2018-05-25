using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkProjectile : MonoBehaviour
{
	public const string resourceName = "Projectile";

	protected Projectile projectile;

	protected Vector3 networkPosition;
	protected Vector3 networkDirection;
	protected double lastNetworkDataReceivedTime = 0;


	protected void Awake ()
	{
		this.projectile = GetComponent<Projectile> ();
	}


	protected virtual void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
	{
		if ( stream.isWriting )
		{
			// This is us updating the projectile
			stream.SendNext (transform.position);
			stream.SendNext (projectile.Direction);
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

		Vector3 newPosition = Vector3.Lerp (transform.position, estimatedPosition, projectile.speed * Time.deltaTime);

		if ( Vector3.SqrMagnitude (estimatedPosition - transform.position) > 4f )
		{
			newPosition = estimatedPosition;
		}

		transform.position = newPosition;
	}


	[PunRPC]
	public void NetworkPrime (Vector3 position, Vector3 direction, float speed, int bounces, int sender, double time)
	{
		float dt = (float) ( PhotonNetwork.time - time );
		transform.position = position + direction * dt * speed;
		projectile.Direction = direction;
		projectile.speed = speed;
		projectile.Bounces = bounces;
	}
}
