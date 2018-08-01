using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Server
{
	#region Statics

	public static Server Current
	{
		get { return new Server (PhotonNetwork.room); }
	}

	#endregion
	
	#region Properties

	public Room Photon
	{
		get;
	}

	public string Name
	{
		get { return Photon.Name; }
	}

	public int PlayerCount
	{
		get { return Photon.PlayerCount; }
	}

	public int MaxPlayers
	{
		get { return Photon.MaxPlayers; }
		set { Photon.MaxPlayers = value; }
	}

	public bool IsVisible
	{
		get { return Photon.IsVisible; }
		set { Photon.IsVisible = value; }
	}

	public bool IsJoinable
	{
		get { return Photon.IsOpen; }
		set { Photon.IsOpen = value; }
	}

	public int InactiveTime
	{
		get { return Photon.PlayerTtl; }
		set { Photon.PlayerTtl = value; }
	}

	#endregion

	public Server (Room room)
	{
		this.Photon = room;
	}

	public void SetProperty (string key, object obj)
	{
		if ( NetworkManager.IsMasterClient == false )
		{
			Debug.LogWarning ($"Attempting to set a room property ({key}) when you are not the master client!");
			//return;
		}

		Photon.SetCustomProperties (new Hashtable () { { key, obj } });
	}

	public T GetProperty<T> (string key, T defaultValue = default (T))
	{
		object obj;
		if ( Photon.CustomProperties.TryGetValue (key, out obj) && obj is T )
		{
			return (T) obj;
		}

		return defaultValue;
	}

	public void ClearProperty (string key)
	{
		SetProperty (key, null);
	}

	#region Team


	public void SetTeamName (int team, string name)
	{
		SetProperty (RoomProperty.Team_Name + team, name);
	}


	public string GetTeamName (int team)
	{
		return GetProperty<string> (RoomProperty.Team_Name + team);
	}


	public void SetTeamSize (int size)
	{
		SetProperty (RoomProperty.Team_MaxSize, size);
	}


	public int GetTeamSize ()
	{
		return GetProperty<int> (RoomProperty.Team_MaxSize);
	}


	public void SetTeamColor (int team, Color color)
	{
		SetProperty (RoomProperty.Team_Color + team, color.ToVector ());
	}


	public Color GetTeamColor (int team)
	{
		return GetProperty (RoomProperty.Team_Color + team, Tank.Default_Color.ToVector ()).ToColor ();
	}


	public void SetTeamScore (int team, int amount)
	{
		SetProperty (RoomProperty.Team_Score + team, amount);
	}


	public int GetTeamScore (int team)
	{
		return GetProperty<int> (RoomProperty.Team_Score + team);
	}


	public void IncreaseTeamScore (int team, int amount)
	{
		SetTeamScore (team, GetTeamScore (team) + amount);
	}

	#endregion
}
