using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTeamValues : MonoBehaviour
{
	public List<string> teamNames;
	public List<Color> teamColors;

	private void Start ()
	{
		for ( int i = 0; i < teamNames.Count; i++ )
		{
			int index = i + 1;

			Server.Current.Photon.SetTeamName (index, teamNames[i]);
		}

		for ( int i = 0; i < teamColors.Count; i++ )
		{
			int index = i + 1;

			Server.Current.Photon.SetTeamColor (index, teamColors[i]);
		}
	}
}
