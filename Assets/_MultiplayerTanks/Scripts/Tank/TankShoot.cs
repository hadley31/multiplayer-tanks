﻿using UnityEngine;

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
	public float radius = 0.075f;
	public float relativeSpeedEffect = 0.5f;


	private float m_LastShootTime = 0;

	public void Shoot ()
	{
		if ( Tank.IsLocal == false )
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

		if (TooCloseToWall () == true)
		{
			return;
		}

		float projectileSpeed = this.speed + Vector3.Dot (Movement.Velocity, spawnPoint.forward) * relativeSpeedEffect;

		ProjectileManager.Instance.SpawnNew (spawnPoint.position, spawnPoint.forward, bounces, radius, damage, health, projectileSpeed, Tank.ID, PhotonNetwork.time);

		m_LastShootTime = Time.realtimeSinceStartup;
	}

	private bool TooCloseToWall ()
	{
		Vector3 start = transform.position;
		Vector3 direction = spawnPoint.position - start;

		start.y += direction.y;
		direction.y = 0;

		float distance = direction.magnitude + radius * 5;

		if ( Physics.Raycast (start, direction, out RaycastHit hitInfo, distance) == false )
		{
			return false;
		}

		if ( hitInfo.transform.gameObject.layer == LayerMask.NameToLayer ("Wall") )
		{
			return true;
		}

		return false;
	}
}
