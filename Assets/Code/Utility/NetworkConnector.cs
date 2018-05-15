using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkConnector : MonoBehaviour
{
	public bool offlineMode = false;
	public bool connectOnStart = true;


	private void Start ()
	{
		PhotonNetwork.offlineMode = offlineMode;
		if (connectOnStart)
		{
			NetworkManager.Connect ();
		}
	}
}
