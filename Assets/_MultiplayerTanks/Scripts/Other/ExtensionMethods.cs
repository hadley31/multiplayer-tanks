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

	public static Vector3 Limit (this Vector3 a, float limit = 1)
	{
		return a.sqrMagnitude > limit ? a.normalized * limit : a;
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

	public static void ClearProperty (this PhotonPlayer player, string key)
	{
		player.SetProperty (key, null);
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

	public static void ClearProperty (this Room room, string key)
	{
		room.SetProperty (key, null);
	}

	public static void SetTankProperty (this Room room, string key, int tankID, object obj)
	{
		room.SetProperty (key + tankID, obj);
	}

	public static T GetTankProperty<T> (this Room room, string key, int tankID, T defaultValue = default (T))
	{
		return room.GetProperty<T> (key + tankID, defaultValue);
	}

	#endregion
}
