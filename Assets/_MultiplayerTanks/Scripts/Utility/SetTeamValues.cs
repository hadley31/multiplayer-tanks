using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTeamValues : MonoBehaviour
{
    public List<string> teamNames;
    public List<Color> teamColors;

    private void Start()
    {
        if (!NetworkManager.IsMasterClient)
        {
            return;
        }

        for (int i = 0; i < teamNames.Count; i++)
        {
            Server.Current.SetTeamName(i + 1, teamNames[i]);
        }

        for (int i = 0; i < teamColors.Count; i++)
        {
            Server.Current.SetTeamColor(i + 1, teamColors[i]);
        }
    }
}
