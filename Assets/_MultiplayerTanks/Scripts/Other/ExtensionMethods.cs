using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public static class ExtensionMethods
{
	#region Transform

	public static void DestroyChildren (this Transform t)
	{
		for ( int i = t.childCount - 1; i >= 0; i-- )
		{
			Object.Destroy (t.GetChild (i).gameObject);
		}
	}

	#endregion

	#region Vector3

	public static Color ToColor (this Vector3 a)
	{
		return new Color (a.x, a.y, a.z);
	}

	public static Vector3 ToVector (this Color a)
	{
		return new Vector3 (a.r, a.g, a.b);
	}

	#endregion

	#region PhotonPlayer

	public static void SetProperty (this PhotonPlayer player, string key, object obj)
	{
		player.SetCustomProperties (new Hashtable () { { key, obj } });
	}

	public static T GetProperty<T> (this PhotonPlayer player, string key, T defaultValue = default (T))
	{
		object obj;
		if ( player.CustomProperties.TryGetValue (key, out obj) && obj is T )
		{
			return (T) obj;
		}
		else
		{
			return defaultValue;
		}
	}

	public static void SetTeam (this PhotonPlayer player, int teamNumber)
	{
		player.SetProperty (PlayerProperty.Team, teamNumber);
	}

	public static Team GetTeam (this PhotonPlayer player)
	{
		string name = player.GetTeamName ();
		int number = player.GetTeamNumber ();
		Color color = player.GetTeamColor ();

		return new Team (name, number, color);
	}

	public static int GetTeamNumber (this PhotonPlayer player)
	{
		return player.GetProperty<int> (PlayerProperty.Team);
	}

	public static string GetTeamName (this PhotonPlayer player)
	{
		return PhotonNetwork.room.GetTeamName (player.GetTeamNumber ());
	}

	public static Color GetTeamColor (this PhotonPlayer player)
	{
		return PhotonNetwork.room.GetTeamColor (player.GetTeamNumber ());
	}

	#region Kills

	public static void SetKills (this PhotonPlayer player, int kills)
	{
		player.SetProperty (PlayerProperty.Kills, kills);
	}

	public static void AddKill (this PhotonPlayer player)
	{
		player.SetProperty (PlayerProperty.Kills, player.GetKills () + 1);
	}

	public static int GetKills (this PhotonPlayer player)
	{
		return player.GetProperty<int> (PlayerProperty.Kills);
	}

	#endregion

	#region Deaths

	public static void SetDeaths (this PhotonPlayer player, int deaths)
	{
		player.SetProperty (PlayerProperty.Deaths, deaths);
	}

	public static void AddDeath (this PhotonPlayer player)
	{
		player.SetProperty (PlayerProperty.Deaths, player.GetDeaths () + 1);
	}

	public static int GetDeaths (this PhotonPlayer player)
	{
		return player.GetProperty<int> (PlayerProperty.Deaths);
	}

	#endregion

	#endregion

	#region Room

	public static void SetProperty (this Room room, string key, object obj)
	{
		room.SetCustomProperties (new Hashtable () { { key, obj } });
	}

	public static T GetProperty<T> (this Room room, string key, T defaultValue = default (T))
	{
		object obj;
		if ( room.CustomProperties.TryGetValue (key, out obj) && obj is T )
		{
			return (T) obj;
		}
		return defaultValue;
	}

	public static void SetTeamName (this Room room, int team, string name)
	{
		if (!PhotonNetwork.isMasterClient)
		{
			Debug.LogWarning ("Attempting to change a team name when you are not the Master Client!");
			return;
		}
		room.SetProperty (RoomProperty.Team_Name + team, name);
	}

	public static string GetTeamName (this Room room, int team)
	{
		return room.GetProperty<string> (RoomProperty.Team_Name + team);
	}

	public static void SetTeamColor (this Room room, int team, Color color)
	{
		if ( !PhotonNetwork.isMasterClient )
		{
			Debug.LogWarning ("Attempting to change a team color when you are not the Master Client!");
			return;
		}
		room.SetProperty (RoomProperty.Team_Color + team, color.ToVector ());
	}

	public static Color GetTeamColor (this Room room, int team)
	{
		return room.GetProperty<Vector3> (RoomProperty.Team_Color + team).ToColor ();
	}

	#endregion
}
