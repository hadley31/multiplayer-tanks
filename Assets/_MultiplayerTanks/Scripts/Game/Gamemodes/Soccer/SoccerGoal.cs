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


		PhotonPlayer player = PhotonView.Find (ball.LastViewToTouch).owner;

		if ( player != null )
		{
			int reward = playerPointsPerGoal;
			if ( player.GetTeamNumber () != team )
			{
				reward = -reward;
			}
			player.AddScore (reward);
		}

		Server.Current.Photon.IncreaseTeamScore (this.team, this.teamPointsPerGoal);

		onScore.Invoke ();

		ball.ResetPosition ();
	}
}
