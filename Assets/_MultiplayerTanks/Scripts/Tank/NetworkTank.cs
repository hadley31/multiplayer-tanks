using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

[RequireComponent(typeof(Tank))]
public class NetworkTank : TankBase
{
    public float moveLerpSpeed = 4;
    public float rotateLerpSpeed = 10;
    public float lookLerpSpeed = 25;


    private Vector3 m_NetworkPosition;
    private Vector3 m_NetworkVelocity;
    private Vector3 m_TargetCursorWorldPos;
    private float m_NetworkRotation;
    private float m_NetworkTopRotation;
    private double m_LastNetworkDataReceivedTime = 0;


    public Vector3 CursorWorldPosition
    {
        get;
        private set;
    }


    public PhotonPlayer Owner
    {
        get { return photonView.owner; }
    }


    private void Start()
    {
        if (!Tank.IsLocal)
        {
            Movement.Rigidbody.interpolation = RigidbodyInterpolation.None;
        }
    }


    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // we are sending data
            stream.SendNext(Movement.Position);
            stream.SendNext(Movement.TargetVelocity);

            stream.SendNext(Movement.Rigidbody.rotation.eulerAngles.y);
            stream.SendNext(Movement.Top.eulerAngles.y);

            stream.SendNext(TankInput.GetLookTarget());
        }
        else
        {
            // we are receiving data
            m_NetworkPosition = (Vector3)stream.ReceiveNext();
            m_NetworkVelocity = (Vector3)stream.ReceiveNext();

            m_NetworkRotation = (float)stream.ReceiveNext();
            m_NetworkTopRotation = (float)stream.ReceiveNext();

            m_TargetCursorWorldPos = (Vector3)stream.ReceiveNext();

            m_LastNetworkDataReceivedTime = info.timestamp;
        }
    }


    private void Update()
    {
        if (!Tank.IsLocal)
        {
            CursorWorldPosition = Vector3.Lerp(CursorWorldPosition, m_TargetCursorWorldPos, Time.deltaTime * 10);

            Movement.SetTargetDirection(m_NetworkVelocity);
        }
    }

    private void LateUpdate()
    {
        if (!Tank.IsLocal)
        {
            UpdateTop();
        }

    }


    private void FixedUpdate()
    {
        if (!Tank.IsLocal)
        {
            Movement.Rigidbody.MovePosition(Vector3.Lerp(transform.position, m_NetworkPosition, Time.deltaTime * 10));
        }
    }


    private void UpdateTop()
    {
        // Rotate the top in update
        Quaternion newTopRotation = Quaternion.Euler(0, m_NetworkTopRotation, 0);
        Movement.Top.rotation = Quaternion.Lerp(Movement.Top.rotation, newTopRotation, Time.deltaTime * lookLerpSpeed);
    }


    private Vector3 GetEstimatedPosition()
    {
        // Get the users ping to the server in seconds
        float pingInSeconds = PhotonNetwork.GetPing() * 0.001f;

        // Calculate the time that has passed since the last OnPhotonSerializeView call
        float timeSinceLastUpdate = (float)(PhotonNetwork.time - m_LastNetworkDataReceivedTime);

        // Add together to get the total time passed
        float totalTimePassed = pingInSeconds + timeSinceLastUpdate;

        // Estimate the position of the tank using linear approximation
        return m_NetworkPosition + (m_NetworkVelocity * totalTimePassed);
    }
}
