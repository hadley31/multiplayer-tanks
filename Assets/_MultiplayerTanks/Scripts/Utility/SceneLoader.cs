﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
	public bool leaveRoomBeforeLoad = false;

	public void LoadScene (string scene)
	{
		if (leaveRoomBeforeLoad)
		{
			NetworkManager.LeaveRoom ();
		}

		SceneManager.LoadScene (scene);
	}

	public void NetworkLoadScene (string scene)
	{
		PhotonNetwork.LoadLevel (scene);
	}
}
