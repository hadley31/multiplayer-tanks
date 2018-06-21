using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (SoccerBall))]
public class SoccerNetworkBall : Photon.MonoBehaviour
{
	public float moveLerpSpeed = 4;

	public float maxPositionError = 10;

	private SoccerBall m_Ball;
	private Rigidbody m_Rigidbody;

	private Vector3 m_NetworkPosition;
	private Vector3 m_NetworkVelocity;
	private double m_LastNetworkDataReceivedTime = 0;

	private void Awake ()
	{
		m_Ball = GetComponent<SoccerBall> ();
		m_Rigidbody = GetComponent<Rigidbody> ();
	}

	private void FixedUpdate ()
	{
		if ( !photonView.isMine )
		{
			// Update this rigidbody's position
			m_Rigidbody.MovePosition (GetLerpedPosition ());
		}
	}

	private void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
	{
		if ( stream.isWriting )
		{
			stream.SendNext (m_Rigidbody.position);
			stream.SendNext (m_Rigidbody.velocity);
		}
		else
		{
			m_NetworkPosition = (Vector3) stream.ReceiveNext ();
			m_NetworkVelocity = (Vector3) stream.ReceiveNext ();

			m_LastNetworkDataReceivedTime = info.timestamp;
		}
	}

	private Vector3 GetLerpedPosition ()
	{
		// Get the users ping to the server in seconds
		float pingInSeconds = PhotonNetwork.GetPing () * 0.001f;

		// Calculate the time that has passed since the last OnPhotonSerializeView call
		float timeSinceLastUpdate = (float) ( PhotonNetwork.time - m_LastNetworkDataReceivedTime );

		// Add together to get the total time passed
		float totalTimePassed = pingInSeconds + timeSinceLastUpdate;

		// Estimate the position of the tank using linear approximation
		Vector3 estimatedPosition = m_NetworkPosition + ( m_NetworkVelocity * totalTimePassed );

		Vector3 lerpedPosition = Vector3.Lerp (m_Rigidbody.position, estimatedPosition, Time.deltaTime * moveLerpSpeed);


		if ( Vector3.SqrMagnitude (lerpedPosition - estimatedPosition) < maxPositionError )
		{
			return lerpedPosition;
		}
		else
		{
			return estimatedPosition;
		}
	}
}
