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
	public TankFollowCamera cameraRigPrefab;

	private Tank tank;

	public override void OnGainControl ()
	{
		Current = this;

		PhotonNetwork.room.SetTeamName (1, "Blue Team");
	//	PhotonNetwork.room.SetTeamColor (1, Color.blue);
		Player.Local.SetTeam (1);

		print (Player.Local.GetTeam ().ToString ());
		ExtDebug.PrintList (Player.AllNames);

		SpawnLocalTank ();
	}

	public void SpawnLocalTank ()
	{
		if ( tank == null )
		{
			tank = PhotonNetwork.Instantiate ("Tank", spawnpoint.position, spawnpoint.rotation, 0).GetComponent<Tank> ();

			tank.name = "Tank_" + Player.Name;

			tank.Visuals.RevertToTeamColor ();

			TankFollowCamera rig = Instantiate (cameraRigPrefab);

			rig.Prime (tank);
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
