using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShoot : Photon.MonoBehaviour
{
	public Projectile projectilePrefab;
	public Transform spawnPoint;
	public float fireRate = 0.018f;

	private float m_lastShootTime = 0;
	private int m_projectileID;

	public void Shoot ()
	{
		if (!photonView.isMine)
		{
			return;
		}

		if ( Time.realtimeSinceStartup - m_lastShootTime < fireRate )
		{
			return;
		}

		if ( NetworkManager.OfflineMode )
		{
			CreateProjectile (spawnPoint.position, spawnPoint.forward, 5, m_projectileID, PhotonNetwork.time);
		}
		else
		{
			photonView.RPC ("CreateProjectile", PhotonTargets.All, spawnPoint.position, spawnPoint.forward, 5f, m_projectileID, PhotonNetwork.time);
		}

		m_projectileID++;
	}

	[PunRPC]
	private void CreateProjectile (Vector3 position, Vector3 direction, float speed, int id, double createTime)
	{
		Projectile p = Instantiate (projectilePrefab, position, Quaternion.identity);
		p.Direction = direction;
	}
}
