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

		List<Tank> team1 = Tank.All.OrderByDescending (x => x.Score).ToList ();

		foreach ( Tank tank in team1 )
		{
			if ( tank.IsPlayer == false )
			{
				continue;
			}

			Instantiate (elementPrefab, content, false).Prime (tank);
		}
	}
}
