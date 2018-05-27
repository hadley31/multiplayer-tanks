using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShoot : Photon.MonoBehaviour
{
	public Projectile projectilePrefab;
	public Transform spawnPoint;
	public float fireRate = 0.018f;

	public int projectileBounces = 1;
	public int projectileDamage = 100;
	public float projectileSpeed = 5;


	private float m_LastShootTime = 0;
	private int m_ProjectileID;

	private List<Projectile> m_Projectiles = new List<Projectile> ();

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

		if ( NetworkManager.OfflineMode )
		{
			CreateProjectile (spawnPoint.position, spawnPoint.forward, projectileBounces, projectileDamage, projectileSpeed, m_ProjectileID, PhotonNetwork.time);
		}
		else
		{
			photonView.RPC ("CreateProjectile", PhotonTargets.All, spawnPoint.position, spawnPoint.forward, projectileBounces, projectileDamage, projectileSpeed, m_ProjectileID, PhotonNetwork.time);
		}

		m_ProjectileID++;
	}

	[PunRPC]
	private void CreateProjectile (Vector3 position, Vector3 direction, int bounces, int damage, float speed, int id, double createTime)
	{
		Projectile newProjectile = Instantiate (projectilePrefab);

		float dt = (float) ( PhotonNetwork.time - createTime );
		position += direction * dt;
		
		newProjectile.SetPosition (position);
		newProjectile.SetSpeed (speed);
		newProjectile.SetDirection (direction);
		newProjectile.SetBounces (bounces);
		newProjectile.SetDamage (damage);
		newProjectile.SetLifeTime (20);
		newProjectile.SetID (id);
		newProjectile.SetOwner (GetComponent<Tank> ());

		m_Projectiles.Add (newProjectile);
	}

	public void DestroyProjectileRPC (int id)
	{
		if ( NetworkManager.OfflineMode )
		{
			DestroyProjectile (id);
		}
		else
		{
			photonView.RPC ("DestroyProjectile", PhotonTargets.All, id);
		}
	}

	[PunRPC]
	public void DestroyProjectile (int id)
	{
		m_Projectiles.RemoveAll (x => x == null);

		Projectile projectile = m_Projectiles.Find (x => x.ID == id);


		if ( projectile != null )
		{
			Destroy (projectile.gameObject);
			m_Projectiles.Remove (projectile);
		}
	}
}
