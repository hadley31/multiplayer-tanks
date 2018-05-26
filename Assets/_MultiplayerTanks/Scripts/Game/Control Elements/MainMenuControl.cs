using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuControl : ControlElement
{
	public override void OnGainControl ()
	{
		NetworkManager.OnJoinLobby += OnConnectedToLobby;
		NetworkManager.OnConnectionFailed += OnConnectToLobbyFailed;
	}

	public override void OnLoseControl ()
	{
		NetworkManager.OnJoinLobby -= OnConnectedToLobby;
		NetworkManager.OnConnectionFailed -= OnConnectToLobbyFailed;
	}

	public override void OnControlUpdate ()
	{

	}

	public void OfflinePlayground ()
	{
		SceneManager.LoadScene ("TestMap", LoadSceneMode.Single);
	}

	public void PlayOnline ()
	{
		GetComponent<CanvasGroup> ().interactable = false;
		NetworkManager.Connect ();
	}


	private void OnConnectedToLobby ()
	{
		NetworkManager.JoinOrCreateRoom ("OD");
	}

	private void OnConnectToLobbyFailed (DisconnectCause cause)
	{
		Debug.LogWarning ("Could not connect! DisconnectCause: " + cause);

		GetComponent<CanvasGroup> ().interactable = true;
	}
}
