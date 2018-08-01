using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelControl : ControlElement
{
	public static GameLevelControl Current
	{
		get;
		private set;
	}

	public Transform spawnpoint;
	public TankFollowCameraRig cameraRigPrefab;

	public override void OnGainControl ()
	{
		Debug.Log ("GameLevelControl gained control");
		Current = this;

		SpawnLocalTank ();

		TankInput.InputOverride = false;

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	public void SpawnLocalTank ()
	{
		if ( TankFollowCameraRig.Instance == null )
		{
			Instantiate (cameraRigPrefab);
		}

		if ( Tank.Local == null )
		{
			PhotonNetwork.Instantiate ("Tank", spawnpoint.position, spawnpoint.rotation, 0).GetComponent<Tank> ();

			Tank.Local.name = "Tank_" + Player.LocalName;

			Tank.Local.Visuals.RevertToTeamColor ();
		}
	}

	public override void OnLoseControl ()
	{
		if (Current == this)
		{
			Current = null;
		}
	}

	public override void OnControlUpdate ()
	{
		TankFollowCameraRig.Instance?.UpdateCursors ();
	}
}
