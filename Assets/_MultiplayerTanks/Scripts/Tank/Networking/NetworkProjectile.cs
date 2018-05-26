using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class NetworkProjectile : PunBehaviour
{
	public const string resourceName = "Projectile";

	protected Projectile projectile;


	protected void Awake ()
	{
		this.projectile = GetComponent<Projectile> ();

		object[] projectileInfo = photonView.instantiationData;

		Vector3 position = (Vector3) projectileInfo[0];
		Vector3 direction = (Vector3) projectileInfo[1];
		float speed = (float) projectileInfo[2];
		int bounces = (int) projectileInfo[3];
		double time = (float) projectileInfo[4];

		float dt = (float) ( PhotonNetwork.time - time );

		projectile.Bounces = bounces;
		transform.position = position + direction * dt * speed;
		projectile.speed = speed;
		projectile.Direction = direction;
	}
}
