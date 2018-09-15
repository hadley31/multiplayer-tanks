using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoccerGamemodeControl : ControlElement
{
	public TeamPickControl pickTeamControl;

	public override void OnGainControl ()
	{
		if ( pickTeamControl.SelectedTeam == 0 )
		{
			pickTeamControl.gameObject.SetActive (true);
			return;
		}

		if ( Tank.Local == null )
		{
			Vector3 spawn = pickTeamControl.SelectedTeam == 1 ? new Vector3 (-18, 0, 0) : new Vector3 (18, 0, 0);
			PhotonNetwork.Instantiate ("SoccerTank", spawn, Quaternion.identity, 0);

			Tank.Local.Team = Server.Current.GetTeam (pickTeamControl.SelectedTeam);

			Tank.Local.Visuals.RevertToTeamColor ();
		}

		TankInput.InputOverride = false;

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	public override void OnControlUpdate ()
	{

	}

	public override void OnLoseControl ()
	{

	}

	public void OnGoalScored (int team)
	{

	}
}
