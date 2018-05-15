using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectToLobby : MonoBehaviour
{
	public void Connect ()
	{
		NetworkManager.Connect ();
	}

	private void Awake ()
	{
		NetworkManager.OnJoinLobby += CreateRoom;
		NetworkManager.OnJoinRoom += ChangeScene;
	}

	private void CreateRoom ()
	{
		NetworkManager.CreateRoom ("Test Room");
	}

	private void ChangeScene ()
	{
		PhotonNetwork.LoadLevel (2);
	}
}
