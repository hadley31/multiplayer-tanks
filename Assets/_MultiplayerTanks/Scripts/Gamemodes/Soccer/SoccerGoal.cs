using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SoccerGoal : MonoBehaviour
{
	public int team;
	public int playerPointsPerGoal = 1;
	public int teamPointsPerGoal = 1;

	public IntUnityEvent onScore;

	public void OnEnter (Entity ent)
	{
		if ( NetworkManager.IsMasterClient == false )
		{
			return;
		}

		if ( ent.Is<SoccerBall> () == false )
		{
			return;
		}

		SoccerBall ball = ent.GetComponent<SoccerBall> ();


		Tank tank = Tank.All.Find (x => x.ID == ball.LastViewToTouch);

		if ( tank != null )
		{
			int reward = tank.Team.Number == team ? playerPointsPerGoal : -playerPointsPerGoal / 2;

			tank.Score += reward;
		}

		
		Server.Current?.IncreaseTeamScore (this.team, this.teamPointsPerGoal);

		onScore.Invoke (team);
	}
}
