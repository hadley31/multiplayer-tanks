using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player
{
	#region Statics

	public static Player Local
	{
		get { return new Player (PhotonNetwork.player); }
	}

	public static List<Player> All
	{
		get { return PhotonNetwork.playerList.Select (x => new Player (x)).ToList (); }
	}

	public static List<Player> Others
	{
		get { return PhotonNetwork.otherPlayers.Select (x => new Player (x)).ToList (); }
	}

	public static string LocalName
	{
		get { return PhotonNetwork.playerName; }
		set { PhotonNetwork.playerName = value; }
	}

	public static List<string> AllNames
	{
		get { return All.Select (x => x.Name).ToList (); }
	}

	public static List<string> OtherNames
	{
		get { return Others.Select (x => x.Name).ToList (); }
	}

	public static Player Find (int id)
	{
		return new Player (PhotonPlayer.Find (id));
	}

	public static Player Find (string name)
	{
		return All.Find (x => x.Name == name);
	}

	public static Player Find (System.Predicate<Player> predicate)
	{
		return All.Find (predicate);
	}

	public static List<Player> FindAll (System.Predicate<Player> predicate)
	{
		return All.FindAll (predicate);
	}

	public static Player FindInOthers (System.Predicate<Player> predicate)
	{
		return Others.Find (predicate);
	}

	public static List<Player> FindAllInOthers (System.Predicate<Player> predicate)
	{
		return Others.FindAll (predicate);
	}

	#endregion

	#region Properties

	public PhotonPlayer Photon
	{
		get;
	}

	public string Name
	{
		get { return Photon.NickName; }
	}

	public bool IsLocal
	{
		get { return Photon.IsLocal; }
	}

	public bool IsMasterClient
	{
		get { return Photon.IsMasterClient; }
	}

	public int ID
	{
		get { return Photon.ID; }
	}

	public int Ping
	{
		get { return Photon.GetPing (); }
	}

	public Team Team
	{
		get { return Photon.GetTeam (); }
		set { Photon.SetTeam (value); }
	}

	#endregion

	public Player (PhotonPlayer photonPlayer)
	{
		this.Photon = photonPlayer;
	}
}
