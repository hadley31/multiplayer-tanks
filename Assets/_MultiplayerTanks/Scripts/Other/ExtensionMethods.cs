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

	public static T GetProperty<T> (this Room room, string key, T onlineDefault = default (T))
	{
		object obj;
		if ( room.CustomProperties.TryGetValue (key, out obj) && obj is T )
		{
			return (T) obj;
		}
		return onlineDefault;
	}

	#endregion
}
