using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

[RequireComponent(typeof(Tank))]
public class NetworkTank : PunBehaviour
{
	private Vector3 networkPosition;
	private Vector3 networkVelocity;
	private float networkRotation;
	private float networkTopRotation;
	private double lastNetworkDataReceivedTime = 0;

	public PhotonPlayer Owner
	{
		get { return photonView.owner; }
	}

	private TankMovement Movement
	{
		get;
		set;
	}

	private void Awake ()
	{
		this.Movement = GetComponent<TankMovement> ();
	}

	private void Update ()
	{
		if (!photonView.isMine)
		{
			UpdateTop ();
		}
	}

	private void FixedUpdate ()
	{
		if ( !photonView.isMine )
		{
			// Update this rigidbody's position
			Movement.Rigidbody.MovePosition (GetLerpedPosition ());

			// Update the rigidbody's rotation
			Quaternion newRotation = Quaternion.Euler (0, networkRotation, 0);
			Movement.Rigidbody.rotation = Quaternion.Lerp (transform.rotation, newRotation, Time.fixedDeltaTime * Movement.rotateSpeed);
		}
	}

	private void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
	{
		if ( stream.isWriting )
		{
			stream.SendNext (Movement.Rigidbody.position);
			stream.SendNext (Movement.Velocity);

			stream.SendNext (Movement.Rigidbody.rotation.eulerAngles.y);
			stream.SendNext (Movement.top.eulerAngles.y);
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

	private void UpdateTop ()
	{
		// Rotate the top in update
		Quaternion newTopRotation = Quaternion.Euler (0, networkTopRotation, 0);
		Movement.top.rotation = Quaternion.Lerp (Movement.top.rotation, newTopRotation, Time.deltaTime * Movement.lookSpeed);
	}

	private Vector3 GetLerpedPosition ()
	{
		// Get the users ping to the server in seconds
		float pingInSeconds = PhotonNetwork.GetPing () * 0.001f;

		// Calculate the time that has passed since the last OnPhotonSerializeView call
		float timeSinceLastUpdate = (float) ( PhotonNetwork.time - lastNetworkDataReceivedTime );

		// Add together to get the total time passed
		float totalTimePassed = pingInSeconds + timeSinceLastUpdate;

		// Estimate the position of the tank using linear approximation
		Vector3 estimatedPosition = networkPosition + ( networkVelocity * totalTimePassed );

		// If the difference between the estimated position and the current position, set the newPosition to the estimated position
		if ( Vector3.SqrMagnitude (estimatedPosition - transform.position) > 2f )
		{
			return estimatedPosition;
		}
		else
		{
			// Otherwise, interpolate the position for a smooth transition
			return Vector3.Lerp (transform.position, estimatedPosition, Time.deltaTime * Movement.moveSpeed);
		}
	}

	public void NetworkDestroy ()
	{
		if (photonView.isMine)
		{
			PhotonNetwork.Destroy (this.gameObject);
		}
	}
}
