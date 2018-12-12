using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class SoccerBall : EntityBase
{
    public float projectileHitForce = 100;
    public float landmineHitForce = 1000;
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

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Collider = GetComponent<Collider>();
    }

    public void ResetPosition()
    {
        if (NetworkManager.IsMasterClient == false)
        {
            return;
        }

        SetPosition(Vector3.up * 2);
    }

    public void SetPosition(Vector3 position)
    {
        if (NetworkManager.IsMasterClient == false)
        {
            return;
        }

        photonView.RPC("SetPositionRPC", PhotonTargets.All, position);
    }

    [PunRPC]
    private void SetPositionRPC(Vector3 position)
    {
        Rigidbody.MovePosition(position);
    }

    private void OnTriggerEnter(Collider other)
    {
        Tank tank = other.GetComponent<Tank>();

        if (tank == null)
        {
            return;
        }

        Vector3 force = (Rigidbody.position - tank.transform.position) * tankHitForce;

        AddForceNetworkTest(tank.ID, force, tank.transform.position);
    }

    private void Update()
    {
        if (NetworkManager.IsMasterClient == false)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            ResetPosition();
        }
    }


    public void AddForceNetworkTest(int id, Vector3 force, Vector3 position)
    {
        if (Tank.Local.ID != id)
        {
            return;
        }

        Rigidbody.AddForceAtPosition(force, position, ForceMode.Acceleration);
        photonView.RPC("AddForceNetworkTestRPC", PhotonTargets.Others, id, Rigidbody.position, Rigidbody.velocity, PhotonNetwork.time);
    }

    [PunRPC]
    private void AddForceNetworkTestRPC(int id, Vector3 ballPos, Vector3 ballVel, double time)
    {
        float dt = (float)(PhotonNetwork.time - time);

        Vector3 newPos = ballPos + ballVel * dt + 0.5f * Physics.gravity * dt * dt;
        Vector3 velocity = ballVel + Physics.gravity * dt;

        Rigidbody.MovePosition(newPos);
        Rigidbody.velocity = velocity;
    }


    public void AddForce(int id, Vector3 force, Vector3 position)
    {
        if (NetworkManager.IsMasterClient)
        {
            Rigidbody.useGravity = true;
            AddForceRPC(id, force, position);
            return;
        }
        else
        {
            Rigidbody.AddForceAtPosition(force, position, ForceMode.Acceleration);
            photonView.RPC("AddForceRPC", PhotonTargets.MasterClient, id, force, position);
        }

    }

    [PunRPC]
    private void AddForceRPC(int id, Vector3 force, Vector3 position)
    {
        Rigidbody.AddForceAtPosition(force, position, ForceMode.Acceleration);
        LastViewToTouch = id;
    }

    public void OnProjectileInteraction(Projectile p)
    {
        if (p.Sender.ID == Tank.Local.ID)
        {
            AddForceNetworkTest(p.Sender.ID, p.Direction * projectileHitForce, p.Rigidbody.position);
        }

        p.Destroy();
    }

    public void OnLandmineInteraction(Landmine landmine)
    {
        Vector3 force = (transform.position - landmine.transform.position).normalized * landmineHitForce;
        Vector3 position = landmine.transform.position;

        AddForceNetworkTest(landmine.Sender.ID, force, position);
    }
}
