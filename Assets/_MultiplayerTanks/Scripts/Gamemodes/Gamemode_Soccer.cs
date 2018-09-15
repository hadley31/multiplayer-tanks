using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemode_Soccer : Gamemode
{
	[PunRPC]
	public override void OnRoundStart ()
	{
		if ( NetworkManager.IsMasterClient )
		{
			PhotonNetwork.Instantiate ("SoccerBall", Vector3.up * 2, Quaternion.identity, 0);
		}
	}

	[PunRPC]
	public override void OnRoundEnd ()
	{
		if ( NetworkManager.IsMasterClient )
		{
			foreach ( SoccerBall go in FindObjectsOfType<SoccerBall> () )
			{
				PhotonNetwork.Destroy (go.gameObject);
			}
		}
	}

	public override string GetShortName ()
	{
		return "SCR";
	}

	public override string ToString ()
	{
		return "Soccer";
	}
}
