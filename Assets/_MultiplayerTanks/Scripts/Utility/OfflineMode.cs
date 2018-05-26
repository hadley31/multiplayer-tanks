using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfflineMode : MonoBehaviour
{
	private void Start ()
	{
		NetworkManager.OfflineMode = true;
	}
}
