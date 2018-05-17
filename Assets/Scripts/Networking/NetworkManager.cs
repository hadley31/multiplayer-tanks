using System;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using ExitGames.Client.Photon;

public class NetworkManager : PunBehaviour
{
	private void Awake ()
	{
		PhotonNetwork.sendRate = 18;
		PhotonNetwork.sendRateOnSerialize = 18;
		DontDestroyOnLoad (this);
	}
	
	#region Events and Delegates

	public delegate void PlayerDelegate (PhotonPlayer player);
	public delegate void DisconnectCauseDelegate (DisconnectCause cause);
	public delegate void HashtableDelegate (Hashtable table);

	public static event Action OnConnectToMaster;
	public static event Action OnConnectToPhoton;
	public static event Action OnDisconnectFromPhoton;
	public static event DisconnectCauseDelegate OnConnectionFailed;
	public static event DisconnectCauseDelegate OnConnectToPhotonFailed;


	public static event Action OnJoinLobby;
	public static event Action OnLeaveLobby;
	public static event Action OnRoomListUpdate;
	public static event Action OnLobbyStatUpdate;


	public static event Action OnRoomCreated;
	public static event Action OnJoinRoom;
	public static event Action OnLeaveRoom;
	public static event Action OnJoinRoomFailed;
	public static event Action OnJoinRandomRoomFailed;
	public static event Action OnCreateRoomFailed;
	public static HashtableDelegate OnRoomPropertiesChanged;

	public static event PlayerDelegate OnOtherPlayerConnect;
	public static event PlayerDelegate OnOtherPlayerDisconnect;
	public static event PlayerDelegate OnNewMasterClient;

	#endregion

	#region Connection

	public static void Connect ()
	{
		PhotonNetwork.ConnectUsingSettings ("v0.0.1");
	}

	public static void Disconnect ()
	{
		PhotonNetwork.Disconnect ();
	}

	#endregion

	#region Connection Messages

	public override void OnConnectedToMaster ()
	{
		if (OnConnectToMaster != null)
		{
			OnConnectToMaster.Invoke ();
		}
	}

	public override void OnConnectedToPhoton ()
	{
		if (OnConnectToPhoton != null)
		{
			OnConnectToPhoton.Invoke ();
		}
	}

	public override void OnDisconnectedFromPhoton ()
	{
		if (OnDisconnectFromPhoton != null)
		{
			OnDisconnectFromPhoton.Invoke ();
		}
	}

	public override void OnConnectionFail (DisconnectCause cause)
	{
		if (OnConnectionFailed != null)
		{
			OnConnectionFailed.Invoke (cause);
		}
	}

	public override void OnFailedToConnectToPhoton (DisconnectCause cause)
	{
		if (OnConnectToPhotonFailed != null)
		{
			OnConnectToPhotonFailed.Invoke (cause);
		}
	}

	#endregion

	#region Lobby

	public static void JoinLobby ()
	{
		PhotonNetwork.JoinLobby ();
	}

	public static void LeaveLobby ()
	{
		PhotonNetwork.LeaveLobby ();
	}

	#endregion

	#region Lobby Messages

	public override void OnJoinedLobby ()
	{
		if (OnJoinLobby != null)
		{
			OnJoinLobby.Invoke ();
		}
	}

	public override void OnLeftLobby ()
	{
		if (OnLeaveLobby != null)
		{
			OnLeaveLobby.Invoke ();
		}
	}

	public override void OnReceivedRoomListUpdate ()
	{
		if (OnRoomListUpdate != null)
		{
			OnRoomListUpdate.Invoke ();
		}
	}

	public override void OnLobbyStatisticsUpdate ()
	{
		if (OnLobbyStatUpdate != null)
		{
			OnLobbyStatUpdate.Invoke ();
		}
	}

	#endregion

	#region Room

	public static void CreateRoom (string name)
	{
		PhotonNetwork.CreateRoom (name);
	}

	public static void JoinRoom (string name)
	{
		PhotonNetwork.JoinRoom (name);
	}

	public static void JoinOrCreateRoom (string name)
	{
		RoomOptions options = new RoomOptions
		{
			IsVisible = true,
			IsOpen = true,
			MaxPlayers = 20,
		};
		PhotonNetwork.JoinOrCreateRoom (name, options, TypedLobby.Default);
	}

	public static void JoinRandomRoom ()
	{
		PhotonNetwork.JoinRandomRoom ();
	}

	public static void LeaveRoom ()
	{
		PhotonNetwork.LeaveRoom ();
	}

	#endregion

	#region Room Messages

	public override void OnCreatedRoom ()
	{
		if (OnRoomCreated != null)
		{
			OnRoomCreated.Invoke ();
		}
	}

	public override void OnPhotonCreateRoomFailed (object[] codeAndMsg)
	{
		if (OnCreateRoomFailed != null)
		{
			OnCreateRoomFailed.Invoke ();
		}
	}

	public override void OnJoinedRoom ()
	{
		if (OnJoinRoom != null)
		{
			OnJoinRoom.Invoke ();
		}
	}

	public override void OnPhotonJoinRoomFailed (object[] codeAndMsg)
	{
		if (OnJoinRoomFailed != null)
		{
			OnJoinRoomFailed.Invoke ();
		}
	}

	public override void OnPhotonRandomJoinFailed (object[] codeAndMsg)
	{
		if (OnJoinRandomRoomFailed != null)
		{
			OnJoinRandomRoomFailed.Invoke ();
		}
	}

	public override void OnLeftRoom ()
	{
		if (OnLeaveRoom != null)
		{
			OnLeaveRoom.Invoke ();
		}
	}

	public override void OnPhotonCustomRoomPropertiesChanged (Hashtable propertiesThatChanged)
	{
		if (OnRoomPropertiesChanged != null)
		{
			OnRoomPropertiesChanged.Invoke (propertiesThatChanged);
		}
	}

	public override void OnPhotonPlayerConnected (PhotonPlayer newPlayer)
	{
		if (OnOtherPlayerConnect != null)
		{
			OnOtherPlayerConnect.Invoke (newPlayer);
		}
	}

	public override void OnPhotonPlayerDisconnected (PhotonPlayer otherPlayer)
	{
		if (OnOtherPlayerDisconnect != null)
		{
			OnOtherPlayerDisconnect.Invoke (otherPlayer);
		}
	}

	public override void OnMasterClientSwitched (PhotonPlayer newMasterClient)
	{
		if (OnNewMasterClient != null)
		{
			OnNewMasterClient.Invoke (newMasterClient);
		}
	}

	#endregion
}
