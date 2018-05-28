using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayPing : MonoBehaviour
{
	private Text text;

	private void OnEnable ()
	{
		text = GetComponent<Text> ();
		if ( NetworkManager.OfflineMode )
		{
			this.gameObject.SetActive (false);
			text.text = string.Empty;
			return;
		}

		
	}

	private void Update ()
	{
		if (text == null)
		{
			return;
		}

		if ( PhotonNetwork.inRoom && !PhotonNetwork.offlineMode )
		{
			text.text = ( PhotonNetwork.GetPing () ).ToString ();
		}
	}
}
