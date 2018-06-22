using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Scoreboard : MonoBehaviour
{
	public ScoreboardElement elementPrefab;

	public RectTransform content;

	public void OnEnable ()
	{
		Prime ();
	}

	public void Prime ()
	{
		content.DestroyChildren ();

		List<Tank> team1 = Tank.All.Where (x => x.Team.Number == 1).OrderByDescending (x => x.Owner.GetScore ()).ToList ();

		foreach ( Tank tank in team1 )
		{
			if ( tank.IsPlayer == false )
			{
				continue;
			}

			Instantiate (elementPrefab, content, false).Prime (tank);
		}

		List<Tank> team2 = Tank.All.FindAll (x => x.Team.Number == 2).OrderByDescending (x => x.Owner.GetScore ()).ToList ();

		foreach ( Tank tank in team2 )
		{
			if ( tank.IsPlayer == false )
			{
				continue;
			}

			Instantiate (elementPrefab, content, false).Prime (tank);
		}
	}
}
