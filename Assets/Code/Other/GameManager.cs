using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public Transform[] team1SpawnPoints;
	public Transform[] team2SpawnPoints;

	public new TankFollowCamera camera;

	private void Awake ()
	{
		// SpawnPlayer ();
		Invoke ("SpawnPlayer", 1);
	}

	[PunRPC]
	public void SpawnPlayer ()
	{
		Tank tank = null;

		tank = PhotonNetwork.Instantiate ("Tank", team1SpawnPoints[Random.Range (0, team1SpawnPoints.Length)].position, Quaternion.identity, 0).GetComponent<Tank> ();

		camera.Prime (tank);
	}

	public void Leave ()
	{
		NetworkManager.LeaveRoom ();
	}
}
