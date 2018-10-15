using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamemodeControl : ControlElement
{
    public static GamemodeControl Current
    {
        get;
        private set;
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

        CrosshairManager.Current?.HideCursor();
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
}
