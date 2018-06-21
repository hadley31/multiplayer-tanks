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
		if ( player.IsLocal == false && PhotonNetwork.isMasterClient == false )
		{
			Debug.LogWarning ($"Attempting to set a player property ({key}) when you are not the master client nor the local player!");
			return;
		}

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

	#region Ping

	public static void UpdatePing (this PhotonPlayer player)
	{
		player.SetProperty (PlayerProperty.Ping, PhotonNetwork.GetPing ());
	}

	public static int GetPing (this PhotonPlayer player)
	{
		return player.GetProperty<int> (PlayerProperty.Ping);
	}

	#endregion

	#region Team

	public static void SetTeam (this PhotonPlayer player, int teamNumber)
	{
		player.SetProperty (PlayerProperty.Team, teamNumber);
	}

	public static Team GetTeam (this PhotonPlayer player)
	{
		return Team.Get (player.GetTeamNumber ());
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

	#endregion

	#region Kills

	public static void SetKills (this PhotonPlayer player, int amount)
	{
		player.SetProperty (PlayerProperty.Kills, amount);
	}

	public static void IncreaseKills (this PhotonPlayer player, int amount = 1)
	{
		player.SetProperty (PlayerProperty.Kills, player.GetKills () + 1);
	}

	public static int GetKills (this PhotonPlayer player)
	{
		return player.GetProperty<int> (PlayerProperty.Kills);
	}

	#endregion

	#region Deaths

	public static void SetDeaths (this PhotonPlayer player, int amount)
	{
		player.SetProperty (PlayerProperty.Deaths, amount);
	}

	public static void IncreaseDeaths (this PhotonPlayer player, int amount = 1)
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
		if ( PhotonNetwork.isMasterClient == false )
		{
			Debug.LogWarning ($"Attempting to set a room property ({key}) when you are not the master client!");
			return;
		}

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

	public static T GetProperty<T> (this RoomInfo room, string key, T defaultValue = default (T))
	{
		object obj;
		if ( room.CustomProperties.TryGetValue (key, out obj) && obj is T )
		{
			return (T) obj;
		}
		return defaultValue;
	}

	#region Map

	public static void SetMap (this Room room, string map)
	{
		room.SetProperty (RoomProperty.Map, map);
	}

	public static string GetMap (this Room room)
	{
		return room.GetProperty (RoomProperty.Map, string.Empty);
	}

	#endregion

	#region Gamemode

	public static void SetGamemode (this Room room, string gamemode)
	{
		room.SetProperty (RoomProperty.Gamemode, gamemode);
	}

	public static string GetGamemode (this Room room)
	{
		return room.GetProperty (RoomProperty.Gamemode, string.Empty);
	}

	#endregion

	#region Team

	public static void SetTeamName (this Room room, int team, string name)
	{
		room.SetProperty (RoomProperty.Team_Name + team, name);
	}

	public static string GetTeamName (this Room room, int team)
	{
		return room.GetProperty<string> (RoomProperty.Team_Name + team);
	}

	public static void SetTeamSize (this Room room, int size)
	{
		room.SetProperty (RoomProperty.Team_MaxSize, size);
	}

	public static int GetTeamSize (this Room room)
	{
		return room.GetProperty<int> (RoomProperty.Team_MaxSize);
	}

	public static void SetTeamColor (this Room room, int team, Color color)
	{
		room.SetProperty (RoomProperty.Team_Color + team, color.ToVector ());
	}

	public static Color GetTeamColor (this Room room, int team)
	{
		return room.GetProperty (RoomProperty.Team_Color + team, Tank.Default_Color.ToVector ()).ToColor ();
	}

	public static void SetTeamScore (this Room room, int team, int amount)
	{
		room.SetProperty (RoomProperty.Team_Score + team, amount);
	}

	public static int GetTeamScore (this Room room, int team)
	{
		return room.GetProperty<int> (RoomProperty.Team_Score + team);
	}

	public static void IncreaseTeamScore (this Room room, int team, int amount)
	{
		room.SetTeamScore (team, room.GetTeamScore (team) + amount);
	}

	#endregion

	#endregion
}
