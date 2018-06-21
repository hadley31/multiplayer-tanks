﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamPickControl : ControlElement
{
	public Image Team1Image;
	public Image Team2Image;
	public Text Team1Text;
	public Text Team2Text;

	public override void OnGainControl ()
	{
		Team1Image.color = GetColor (1);
		Team2Image.color = GetColor (2);
		Team1Text.text = $"Join {GetName (1)}";
		Team2Text.text = $"Join {GetName (2)}";
	}

	public override void OnControlUpdate ()
	{

	}

	public override void OnLoseControl ()
	{

	}

	public void SelectTeam (int team)
	{
		Player.Local.Photon.SetTeam (team);
		gameObject.SetActive (false);
	}

	private string GetName (int team)
	{
		return Server.Current?.Photon.GetTeamName (team) ?? $"Team {team}";
	}

	private Color GetColor (int team)
	{
		return Server.Current?.Photon.GetTeamColor (team) ?? Tank.Default_Color;
	}
}
