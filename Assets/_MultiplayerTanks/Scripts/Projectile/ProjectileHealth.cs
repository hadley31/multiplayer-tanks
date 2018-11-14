using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHealth : Health
{
    public override void SetValue(int value)
    {
        if (NetworkManager.IsMasterClient == false)
        {
            return;
        }

        SetValueRPC(value);
    }

    public override void SetMaxValue(int maxValue, bool setValueToMax = false)
    {
        if (NetworkManager.IsMasterClient == false)
        {
            return;
        }

        SetMaxValueRPC(maxValue);
    }
}
