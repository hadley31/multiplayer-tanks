using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShoot : TankBase
{
	[Header ("Spawn Info")]
	public Projectile projectilePrefab;
	public Transform spawnPoint;

	[Header ("Use Info")]
	public float fireRate = 0.018f;

	[Header ("Projectile Info")]
	public int bounces = 1;
	public int damage = 100;
	public int health = 1;
	public float speed = 5;


	private float m_LastShootTime = 0;

	public void Shoot ()
	{
		if ( photonView.isMine == false )
		{
			return;
		}

		if ( Tank.IsAlive == false )
		{
			return;
		}

		if ( Time.realtimeSinceStartup - m_LastShootTime < fireRate )
		{
			return;
		}

		int id = ProjectileManager.GetNextID ();

		ProjectileManager.Instance.SpawnNewRPC (spawnPoint.position, spawnPoint.forward, bounces, damage, health, speed, photonView.viewID, id, PhotonNetwork.time);

		m_LastShootTime = Time.realtimeSinceStartup;
	}
}
