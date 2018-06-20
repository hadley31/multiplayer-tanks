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

	private Tank tank;

	public override void OnGainControl ()
	{
		Current = this;

		PhotonNetwork.room.SetTeamName (1, "Blue Team");
		PhotonNetwork.room.SetTeamColor (1, new Color (0.3137f, 0.2627f, 0.8588f));
		Player.Local.Photon.SetTeam (1);

		print (Player.Local.Photon.GetTeam ().ToString ());
		ExtDebug.PrintList (Player.AllNames);

		SpawnLocalTank ();
	}

	public void SpawnLocalTank ()
	{
		if ( TankFollowCameraRig.Instance == null )
		{
			Instantiate (cameraRigPrefab);
		}

		if ( tank == null )
		{
			tank = PhotonNetwork.Instantiate ("Tank", spawnpoint.position, spawnpoint.rotation, 0).GetComponent<Tank> ();

			tank.name = "Tank_" + Player.LocalName;

			tank.Visuals.RevertToTeamColor ();
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

	}
}
