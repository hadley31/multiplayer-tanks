using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemode_Soccer : Gamemode
{
	[PunRPC]
	public override void OnRoundStart ()
	{
		if (PhotonNetwork.isMasterClient)
		{
			PhotonNetwork.Instantiate ("SoccerBall", Vector3.up * 2, Quaternion.identity, 0);
		}
	}

	[PunRPC]
	public override void OnRoundEnd ()
	{
		if (PhotonNetwork.isMasterClient)
		{
			foreach (SoccerBall go in GameObject.FindObjectsOfType<SoccerBall>())
			{
				PhotonNetwork.Destroy (go.gameObject);
			}
		}
	}

	[PunRPC]
	public override string GetShortName ()
	{
		return "SCR";
	}

	[PunRPC]
	public override string ToString ()
	{
		return "Soccer";
	}
}
