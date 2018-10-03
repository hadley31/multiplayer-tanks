using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Gamemode : Photon.MonoBehaviour
{
    public void OnRoundStartRPC()
    {
        if (NetworkManager.IsMasterClient == false)
        {
            return;
        }

        photonView.RPC("OnRoundStart", PhotonTargets.AllBuffered);
    }

    public void OnRoundEndRPC()
    {
        if (NetworkManager.IsMasterClient == false)
        {
            return;
        }

        photonView.RPC("OnRoundEnd", PhotonTargets.AllBuffered);
    }

    [PunRPC]
    public abstract void OnRoundStart();
    [PunRPC]
    public abstract void OnRoundEnd();


    public virtual bool ProjectileDoesDamage(Projectile p, Health h)
    {
        if (h.Entity.Is<Tank>())
        {
            return p.Sender.Team != h.GetComponent<Tank>().Team;
        }

        if (h.Entity.Is<Landmine>())
        {
            return true;
        }

        if (h.Entity.Is<DestructibleWall>())
        {
            return false;
        }

        return true;
    }


    public virtual bool LandmineDoesDamage(Landmine mine, Tank tank)
    {
        return true;
    }


    public virtual void SpawnLocalTank(Vector3 position)
    {
		
    }


    public abstract string GetShortName();
    public abstract override string ToString();
}
