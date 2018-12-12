using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class SoccerGamemodeControl : GamemodeControl
{
    protected override string TankName
    {
        get { return "Soccer Tank"; }
    }

    public int playerPointsPerGoal = 5;
    public int teamPointsPerGoal = 1;

    public TeamSelectMenuControl teamSelectControl;
    public IntUnityEvent onGoalScored;

    public override void OnGainControl()
    {
        if (teamSelectControl.SelectedTeam < 0)
        {
            teamSelectControl.gameObject.SetActive(true);
            return;
        }

        if (Tank.Local != null)
        {
            CrosshairManager.Current?.HideCursor();
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
        if (!NetworkManager.IsMasterClient)
        {
            return;
        }

        GetComponent<PhotonView>().RPC("OnGoalScoredRPC", PhotonTargets.All, team);
    }

    [PunRPC]
    private void OnGoalScoredRPC(int team)
    {
        onGoalScored.Invoke(team);
    }

    public void OnChangeTeam(int team)
    {
        if (team < 0)
        {
            return;
        }
        else if (team == 0)
        {
            if (Tank.Local != null)
            {
                Tank.Local.Remove();
            }

            print("Spectating!");

            CrosshairManager.Current?.ShowCursor();

            return;
        }

        if (Tank.Local == null)
        {

            Tank tank = CreateTank();

            tank.SetTeam(teamSelectControl.SelectedTeam);

            CrosshairManager.Current?.HideCursor();
        }
        else
        {
            Tank.Local.SetTeam(team);
        }

        EntityFollowCamera.Instance?.OnlyFollow(Tank.Local.Entity);
    }

    public override Vector3 GetSpawnPoint()
    {
        return Vector3.right * 18 * (teamSelectControl.SelectedTeam == 1 ? -1 : 1) + Vector3.forward * Random.Range(-5, 5);
    }
}
