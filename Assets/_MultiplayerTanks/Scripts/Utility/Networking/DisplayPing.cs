using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayPing : MonoBehaviour
{
	private Text text;

	private void Start ()
	{
		if (NetworkManager.OfflineMode)
		{
			this.enabled = false;
			return;
		}

		text = GetComponent<Text> ();
	}

	private void Update ()
	{
		if (text == null)
		{
			return;
		}

		if (PhotonNetwork.inRoom && !PhotonNetwork.offlineMode)
		{
			text.text = (PhotonNetwork.GetPing ()).ToString ();
		}
	}
}
