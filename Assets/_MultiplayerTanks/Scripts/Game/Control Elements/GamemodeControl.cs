using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GamemodeControl : ControlElement
{
    public static GamemodeControl Current
    {
        get;
        private set;
    }

    protected virtual string TankName
    {
        get { return "Player Tank"; }
    }

    public override void OnGainControl()
    {
        if (Current != null)
        {
            Destroy(gameObject);
            return;
        }

        Debug.Log("GameLevelControl gained control");
        Current = this;
    }

    public override void OnLoseControl()
    {
        if (Current == this)
        {
            Current = null;
        }

        CrosshairManager.Current?.ShowCursor();
    }

    public override void OnControlUpdate()
    {

    }

    public virtual Tank CreateTank()
    {
        Tank tank = PhotonNetwork.Instantiate(TankName, GetSpawnPoint(), Quaternion.identity, 0).GetComponent<Tank>();

        tank.Visuals.RevertToTeamColor();

        return tank;
    }

    public abstract Vector3 GetSpawnPoint();
}
