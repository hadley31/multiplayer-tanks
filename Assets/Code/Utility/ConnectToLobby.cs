using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectToLobby : MonoBehaviour
{
	public void Connect ()
	{
		NetworkManager.Connect ();
	}

	private void OnEnable ()
	{
		NetworkManager.OnJoinLobby += CreateRoom;
		NetworkManager.OnJoinRoom += ChangeScene;
	}

	private void OnDisable ()
	{
		NetworkManager.OnJoinLobby -= CreateRoom;
		NetworkManager.OnJoinRoom -= ChangeScene;
	}

	private void CreateRoom ()
	{
		NetworkManager.JoinOrCreateRoom ("Test Room");
	}

	private void ChangeScene ()
	{
		PhotonNetwork.LoadLevel (1);
	}
}
