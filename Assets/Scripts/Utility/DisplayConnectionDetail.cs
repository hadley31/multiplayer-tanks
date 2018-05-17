using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayConnectionDetail : MonoBehaviour
{
	public Text displayText;

	private void Update ()
	{
		displayText.text = PhotonNetwork.connectionStateDetailed.ToString ();
	}
}
