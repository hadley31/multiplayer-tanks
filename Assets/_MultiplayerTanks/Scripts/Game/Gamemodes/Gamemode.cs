using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Gamemode : Photon.MonoBehaviour
{
	public void OnRoundStartRPC ()
	{
		if (PhotonNetwork.isMasterClient == false)
		{
			return;
		}

		photonView.RPC ("OnRoundStart", PhotonTargets.AllBuffered);
	}

	public void OnRoundEndRPC ()
	{
		if ( PhotonNetwork.isMasterClient == false )
		{
			return;
		}

		photonView.RPC ("OnRoundEnd", PhotonTargets.AllBuffered);
	}

	[PunRPC]
	public abstract void OnRoundStart ();
	[PunRPC]
	public abstract void OnRoundEnd ();

	public virtual bool ProjectileDoesDamage (Projectile p, Tank tank)
	{
		return p.Sender?.Team != tank?.Team;
	}

	public virtual bool LandmineDoesDamage (Landmine mine, Tank tank)
	{
		return true;
	}



	public abstract string GetShortName ();
	public abstract override string ToString ();
}
