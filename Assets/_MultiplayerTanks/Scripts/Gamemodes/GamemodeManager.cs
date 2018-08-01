using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamemodeManager : Photon.MonoBehaviour
{
	public static GamemodeManager Instance
	{
		get;
		private set;
	}

	public Gamemode ActiveGamemode
	{
		get;
		private set;
	}

	public List<Gamemode> AvailableGamemodes
	{
		get;
		private set;
	}

	private void OnEnable ()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Debug.Log ("A Gamemode Manager instance is already active! Destroying new instance.");
			Destroy (this);
		}
	}

	private void OnDisable ()
	{
		if ( Instance == this )
		{
			Instance = null;
		}
	}

	public void SetActiveGamemodeRPC (string name)
	{
		photonView.RPC ("SetActiveGamemode", PhotonTargets.AllBufferedViaServer, name);
	}

	[PunRPC]
	public void SetActiveGamemode (string name)
	{
		this.ActiveGamemode = AvailableGamemodes.Find (x => x.GetShortName () == name);


	}

	public T GetActiveGamemode<T> () where T : Gamemode
	{
		return this.ActiveGamemode as T;
	}
}
