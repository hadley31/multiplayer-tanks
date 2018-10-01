using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemode_FreeForAll : Gamemode
{
    public override void OnRoundStart()
    {
        throw new System.NotImplementedException();
    }

    public override void OnRoundEnd()
    {
        throw new System.NotImplementedException();
    }

    public override string GetShortName()
    {
        return "FFA";
    }

    public override string ToString()
    {
        return "Free For All";
    }
}
