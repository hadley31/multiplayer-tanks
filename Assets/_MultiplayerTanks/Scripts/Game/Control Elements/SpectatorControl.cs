using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectatorControl : ControlElement
{
    public int mask
    {
        get { return LayerMask.GetMask("Tank"); }
    }

    public override void OnGainControl()
    {

    }

    public override void OnLoseControl()
    {

    }

    public override void OnControlUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = TankFollowCameraRig.Instance.Camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 100, mask) == true)
            {
                Tank tank = hitInfo.transform.GetComponent<Tank>();
                if (tank != null)
                {
                    TankFollowCameraRig.Instance?.ToggleFollow(tank);
                }
            }
        }
    }
}
