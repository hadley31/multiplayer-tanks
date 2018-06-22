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
	public float relativeSpeedEffect = 0.5f;


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

		if (TooCloseToWall () == true)
		{
			return;
		}

		int id = ProjectileManager.GetNextID ();

		float projectileSpeed = this.speed + Vector3.Dot (Movement.Velocity, spawnPoint.forward) * relativeSpeedEffect;

		ProjectileManager.Instance.SpawnNewRPC (spawnPoint.position, spawnPoint.forward, bounces, damage, health, projectileSpeed, Tank.ID, id, PhotonNetwork.time);

		m_LastShootTime = Time.realtimeSinceStartup;
	}

	public bool TooCloseToWall ()
	{
		Vector3 start = transform.position;
		Vector3 direction = spawnPoint.position - start;

		start.y += direction.y;
		direction.y = 0;

		float distance = direction.magnitude + Projectile.Radius * 5;

		RaycastHit hitInfo;
		if ( Physics.Raycast (start, direction, out hitInfo, distance) == false)
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
