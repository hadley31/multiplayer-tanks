using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RoomProperty
{
	public static readonly string Team_MaxSize = "tp";
	public static readonly string Team_Name = "tn";
	public static readonly string Team_Color = "tc";
	public static readonly string Map = "mp";
	public static readonly string Gamemode = "gm";


	public static string GetTeamProp (int number)
	{
		return Team_Name + number;
	}
}

public static class PlayerProperty
{
	public static readonly string Ping = "p";
	public static readonly string Kills = "k";
	public static readonly string Deaths = "d";
	public static readonly string Team = "t";
}