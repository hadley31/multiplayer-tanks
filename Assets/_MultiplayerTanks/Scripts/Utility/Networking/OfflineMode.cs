using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfflineMode : MonoBehaviour
{
	public bool createRoomOnAwake = true;

	private void Awake ()
	{
		if (PhotonNetwork.inRoom)
		{
			Debug.LogWarning ("You are already in a room! Offline mode canceled.");
			return;
		}

		NetworkManager.OfflineMode = true;

		if ( createRoomOnAwake )
		{
			NetworkManager.CreateRoom ("Offline");
		}
	}

	private void OnDestroy ()
	{
		if (NetworkManager.OfflineMode && NetworkManager.InRoom)
		{
			NetworkManager.LeaveRoom ();
		}
	}
}
