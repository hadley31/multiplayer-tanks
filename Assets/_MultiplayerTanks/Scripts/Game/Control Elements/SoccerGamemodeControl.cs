using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoccerGamemodeControl : GamemodeControl
{
    protected override string TankName
    {
        get { return "Tank_Soccer"; }
    }

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
            Vector3 spawn = Vector3.right * 18 * (teamSelectControl.SelectedTeam == 1 ? -1 : 1);
            Tank tank = CreateTank();

            tank.transform.position = spawn;
            tank.SetTeam(teamSelectControl.SelectedTeam);
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

    public void OnChangeTeam(int team)
    {

    }
}
