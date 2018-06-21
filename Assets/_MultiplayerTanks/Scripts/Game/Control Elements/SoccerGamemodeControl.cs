using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoccerGamemodeControl : ControlElement
{
	public TeamPickControl pickTeamControl;

	public override void OnGainControl ()
	{
		if (Player.Local.Team.Number == 0)
		{
			pickTeamControl.gameObject.SetActive (true);
			return;
		}

		if (Tank.Local == null)
		{
			Vector3 spawn = Player.Local.Team.Number == 1 ? new Vector3 (-18, 0, 0) : new Vector3 (18, 0, 0);
			PhotonNetwork.Instantiate ("SoccerTank", spawn, Quaternion.identity, 0);

			Tank.Local.Visuals.RevertToTeamColor ();
		}
	}

	public override void OnControlUpdate ()
	{

	}

	public override void OnLoseControl ()
	{

	}
}
