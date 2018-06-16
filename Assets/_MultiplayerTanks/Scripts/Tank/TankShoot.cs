using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShoot : TankBase
{
	public Projectile projectilePrefab;
	public Transform spawnPoint;
	public float fireRate = 0.018f;

	public int projectileBounces = 1;
	public int projectileDamage = 100;
	public float projectileSpeed = 5;


	private float m_LastShootTime = 0;

	public void Shoot ()
	{
		if (!photonView.isMine)
		{
			return;
		}

		if ( Time.realtimeSinceStartup - m_LastShootTime < fireRate )
		{
			return;
		}

		int id = ProjectileManager.GetNextID ();

		ProjectileManager.Instance.SpawnNewRPC (spawnPoint.position, spawnPoint.forward, projectileBounces, projectileDamage, projectileSpeed, id, PhotonNetwork.time);

		m_LastShootTime = Time.realtimeSinceStartup;
	}
}
