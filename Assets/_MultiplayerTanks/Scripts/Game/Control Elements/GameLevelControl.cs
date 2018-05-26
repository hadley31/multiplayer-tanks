using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelControl : ControlElement
{
	public Transform spawnpoint;
	public TankFollowCamera cameraRigPrefab;

	private Tank tank;

	public override void OnGainControl ()
	{
		if (tank == null)
		{
			tank = PhotonNetwork.Instantiate ("Tank", spawnpoint.position, spawnpoint.rotation, 0).GetComponent<Tank> ();

			TankFollowCamera rig = Instantiate (cameraRigPrefab);

			rig.Prime (tank);
		}
	}

	public override void OnLoseControl ()
	{

	}

	public override void OnControlUpdate ()
	{

	}
}
