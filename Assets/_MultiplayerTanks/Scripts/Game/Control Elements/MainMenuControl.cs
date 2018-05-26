using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuControl : ControlElement
{
	public override void OnGainControl ()
	{
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

	public void OfflinePlayground ()
	{
		NetworkManager.OfflineMode = true;
		NetworkManager.CreateRoom ("Offline");
	}

	public void PlayOnline ()
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
		PhotonNetwork.LoadLevel ("TestMap");
	}

	private void OnConnectToLobbyFailed (DisconnectCause cause)
	{
		Debug.LogWarning ("Could not connect! DisconnectCause: " + cause);

		GetComponent<CanvasGroup> ().interactable = true;
	}
}
