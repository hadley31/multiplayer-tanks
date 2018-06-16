using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Player
{
	public static string Name
	{
		get { return PhotonNetwork.playerName; }
		set { PhotonNetwork.playerName = value; }
	}

	public static PhotonPlayer Local
	{
		get { return PhotonNetwork.player; }
	}

	public static List<PhotonPlayer> All
	{
		get { return PhotonNetwork.playerList.ToList (); }
	}

	public static List<PhotonPlayer> Others
	{
		get { return PhotonNetwork.otherPlayers.ToList (); }
	}

	public static List<string> OtherNames
	{
		get { return Others.Select (x => x.NickName).ToList (); }
	}

	public static List<string> AllNames
	{
		get { return All.Select (x => x.NickName).ToList (); }
	}

	public static PhotonPlayer Find (int id)
	{
		return PhotonPlayer.Find (id);
	}

	public static PhotonPlayer Find (string name)
	{
		return All.Find (x => x.NickName == name);
	}

	public static PhotonPlayer Find (System.Predicate<PhotonPlayer> predicate)
	{
		return All.Find (predicate);
	}

	public static List<PhotonPlayer> FindAll (System.Predicate<PhotonPlayer> predicate)
	{
		return All.FindAll (predicate);
	}

	public static PhotonPlayer FindInOthers (System.Predicate<PhotonPlayer> predicate)
	{
		return Others.Find (predicate);
	}

	public static List<PhotonPlayer> FindAllInOthers (System.Predicate<PhotonPlayer> predicate)
	{
		return Others.FindAll (predicate);
	}
}
