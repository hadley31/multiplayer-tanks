using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

[RequireComponent(typeof(Tank))]
public class NetworkTank : PunBehaviour
{
	protected Tank tank;

	protected Vector3 networkPosition;
	protected Vector3 networkVelocity;
	protected float networkRotation;
	protected float networkTopRotation;
	protected double lastNetworkDataReceivedTime = 0;

	public PhotonPlayer owner
	{
		get { return photonView.owner; }
	}

	protected void Awake ()
	{
		this.tank = GetComponent<Tank> ();

		// If the photonView does is not already observing this component, add it to the list
		if (!photonView.ObservedComponents.Contains (this))
		{
			photonView.ObservedComponents.Add (this);
		}
		
	}

	protected void Update ()
	{
		if (!photonView.isMine)
		{
			PhotonUpdate ();
		}
	}

	protected void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
	{
		if ( stream.isWriting )
		{
			stream.SendNext (transform.position);
			stream.SendNext (tank.rb.velocity);

			stream.SendNext (transform.eulerAngles.y);
			stream.SendNext (tank.top.eulerAngles.y);
		}
		else
		{
			networkPosition = (Vector3) stream.ReceiveNext ();
			networkVelocity = (Vector3) stream.ReceiveNext ();

			networkRotation = (float) stream.ReceiveNext ();
			networkTopRotation = (float) stream.ReceiveNext ();

			lastNetworkDataReceivedTime = info.timestamp;
		}
	}

	protected void PhotonUpdate ()
	{
		// Get the users ping to the server in seconds
		float pingInSeconds = PhotonNetwork.GetPing () * 0.001f;

		// Calculate the time that has passed since the last OnPhotonSerializeView call
		float timeSinceLastUpdate = (float) ( PhotonNetwork.time - lastNetworkDataReceivedTime );

		// Add together to get the total time passed
		float totalTimePassed = pingInSeconds + timeSinceLastUpdate;

		// Estimate the position of the tank using linear approximation
		Vector3 estimatedPosition = networkPosition + ( networkVelocity * totalTimePassed );

		// Interpolate the position for a smooth transition
		Vector3 newPosition = Vector3.Lerp (transform.position, estimatedPosition, Time.deltaTime * 3); // TODO: moveSpeed

		// If the difference between the estimated position and the current position, set the newPosition to the estimated position 
		if ( Vector3.SqrMagnitude (estimatedPosition - transform.position) > 25f )
		{
			newPosition = estimatedPosition;
		}

		transform.position = newPosition;

		Quaternion newRotation = Quaternion.Euler (0, networkRotation, 0);
		Quaternion newTopRotation = Quaternion.Euler (0, networkTopRotation, 0);

		transform.rotation = Quaternion.Lerp (transform.rotation, newRotation, Time.deltaTime * 10); // TODO: turnSpeed
		tank.top.rotation = Quaternion.Lerp (tank.top.rotation, newTopRotation, Time.deltaTime * 45); // TODO: topRotateSpeed
	}
}
