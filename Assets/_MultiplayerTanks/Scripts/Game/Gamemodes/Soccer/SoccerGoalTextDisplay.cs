﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SoccerGoalTextDisplay : MonoBehaviour
{
	public float duration = 1.5f;

	public TextMeshProUGUI goalText;
	public TextMeshProUGUI team1Score;
	public TextMeshProUGUI team2Score;

	private void Awake ()
	{
		gameObject.SetActive (false);
	}

	public void UpdateText (int team)
	{
		team1Score.color = GetColor (1);
		team2Score.color = GetColor (2);

		team1Score.text = GetScore (1).ToString ();
		team2Score.text = GetScore (2).ToString ();

		if ( team == 1 )
		{
			goalText.color = GetColor (1);
			goalText.text = GetName (1) + " Scored";
		}
		else
		{
			goalText.color = GetColor (2);
			goalText.text = GetName (2) + " Scored";
		}

		Display ();
	}

	public void Display ()
	{
		gameObject.SetActive (true);
		StartCoroutine (DisplayText ());
	}

	private IEnumerator DisplayText ()
	{
		yield return new WaitForSecondsRealtime (duration);

		gameObject.SetActive (false);
	}

	private string GetName (int team)
	{
		return Server.Current?.Photon.GetTeamName (team) ?? $"Team {team}";
	}

	private int GetScore (int team)
	{
		return Server.Current?.Photon.GetTeamScore (team) ?? 0;
	}

	private Color GetColor (int team)
	{
		return Server.Current?.Photon.GetTeamColor (team) ?? Tank.Default_Color;
	}
}
