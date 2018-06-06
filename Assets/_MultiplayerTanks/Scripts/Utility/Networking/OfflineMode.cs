using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfflineMode : MonoBehaviour
{
	public bool onlyIfNotConnected = true;
	public bool createRoomOnAwake = true;

	private void Awake ()
	{
		if (PhotonNetwork.inRoom)
		{
			Debug.LogWarning ("Offline mode canceled becuase you are already in a room!");
			return;
		}

		NetworkManager.OfflineMode = true;

		if ( createRoomOnAwake )
		{
			NetworkManager.CreateRoom ("Offline_Room");
		}
	}
}
