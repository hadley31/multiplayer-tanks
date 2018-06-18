using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ServerInfo : MonoBehaviour
{
	public static List<ServerInfo> GetServerList ()
	{
		return PhotonNetwork.GetRoomList ().Select (x => new ServerInfo (x)).ToList ();
	}

	public RoomInfo Photon
	{
		get;
	}

	public string Name
	{
		get { return Photon.Name; }
	}

	public ServerInfo (RoomInfo info)
	{
		this.Photon = info;
	}
}
