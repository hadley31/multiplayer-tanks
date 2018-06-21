﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuControl : ControlElement
{
	public override void OnGainControl ()
	{
		if ( NetworkManager.IsConnected )
		{
			NetworkManager.Disconnect ();
		}

		NetworkManager.OnJoinLobby += OnConnectedToLobby;
		NetworkManager.OnJoinRoom += OnJoinRoom;
		NetworkManager.OnConnectionFailed += OnConnectToLobbyFailed;
	}

	public override void OnLoseControl ()
	{
		NetworkManager.OnJoinLobby -= OnConnectedToLobby;
		NetworkManager.OnJoinRoom -= OnJoinRoom;
		NetworkManager.OnConnectionFailed -= OnConnectToLobbyFailed;
	}

	public override void OnControlUpdate ()
	{

	}

	public void PlayOffline (string level)
	{
		NetworkManager.OfflineMode = true;
		NetworkManager.CreateRoom (level);
	}

	public void PlayOnline (string level)
	{
		NetworkManager.OfflineMode = false;
		GetComponent<CanvasGroup> ().interactable = false;
		NetworkManager.Connect ();
	}


	private void OnConnectedToLobby ()
	{
		NetworkManager.JoinOrCreateRoom ("OD");
	}

	private void OnJoinRoom ()
	{
		if (NetworkManager.OfflineMode)
		{
			PhotonNetwork.LoadLevel (PhotonNetwork.room.Name);
		}
		else
		{
			PhotonNetwork.LoadLevel ("TestMap");
		}
	}

	private void OnConnectToLobbyFailed (DisconnectCause cause)
	{
		Debug.LogWarning ("Could not connect! DisconnectCause: " + cause);

		GetComponent<CanvasGroup> ().interactable = true;
	}
}
