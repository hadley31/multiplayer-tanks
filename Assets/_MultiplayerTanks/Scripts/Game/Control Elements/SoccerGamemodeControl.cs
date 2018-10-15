using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoccerGamemodeControl : GamemodeControl
{
    public TeamSelectMenuControl teamSelectControl;

    public override void OnGainControl()
    {
        if (teamSelectControl.SelectedTeam == 0)
        {
            teamSelectControl.gameObject.SetActive(true);
            return;
        }

        if (Tank.Local == null)
        {
            Vector3 spawn = teamSelectControl.SelectedTeam == 1 ? new Vector3(-18, 0, 0) : new Vector3(18, 0, 0);
            PhotonNetwork.Instantiate("Tank_Soccer", spawn, Quaternion.identity, 0);

            Tank.Local.Team = Server.Current.GetTeam(teamSelectControl.SelectedTeam);

            Tank.Local.Visuals.RevertToTeamColor();
        }

        base.OnGainControl();
    }

    public override void OnControlUpdate()
    {

    }

    public override void OnLoseControl()
    {
        base.OnLoseControl();
    }

    public void OnGoalScored(int team)
    {

    }
}
