using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SoccerGoal : MonoBehaviour
{
	public int team;
	public int playerPointsPerGoal = 1;
	public int teamPointsPerGoal = 1;

	public UnityEvent onScore;

	public void OnEnter (Entity ent)
	{
		if ( PhotonNetwork.isMasterClient == false )
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
			int reward = playerPointsPerGoal;
			if ( tank.Team.Number != team )
			{
				reward = -reward;
			}
			tank.Kills++;
		}

		Server.Current.IncreaseTeamScore (this.team, this.teamPointsPerGoal);

		onScore.Invoke ();

		ball.ResetPosition ();
	}
}
