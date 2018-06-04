using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (ObjectPool), typeof (PhotonView))]
public class ProjectileManager : Photon.MonoBehaviour
{
	public static ProjectileManager Instance
	{
		get;
		private set;
	}

	public static int GetNextID ()
	{
		return Instance.m_ProjectileID++;
	}

	private int m_ProjectileID = 0;
	private ObjectPool m_Pool;
	private List<Projectile> m_Projectiles = new List<Projectile> ();

	private void Awake ()
	{
		m_Pool = GetComponent<ObjectPool> ();
	}

	private void OnEnable ()
	{
		if ( Instance == null )
		{
			Instance = this;
		}
		else
		{
			Destroy (this);
		}
	}

	private void OnDisable ()
	{
		if ( Instance == this )
		{
			Instance = null;
		}
	}
	
	public void SpawnNewRPC (Vector3 position, Vector3 direction, int bounces, int damage, float speed, int id, double createTime)
	{
		photonView.RPC ("SpawnNew", PhotonTargets.All, position, direction, bounces, damage, speed, id, createTime);
	}

	[PunRPC]
	public void SpawnNew (Vector3 position, Vector3 direction, int bounces, int damage, float speed, int id, double createTime)
	{
		Projectile p = m_Pool.Spawn<Projectile> ();

		float dt = (float) ( PhotonNetwork.time - createTime );
		position += direction * dt;

		p.SetPosition (position);
		p.SetSpeed (speed);
		p.SetDirection (direction);
		p.SetBounces (bounces);
		p.SetDamage (damage);
		p.SetLifeTime (20);
		p.SetID (id);

		m_Projectiles.Add (p);
	}

	public void DestroyRPC (int id)
	{
		photonView.RPC ("Destroy", PhotonTargets.All, id);
	}

	[PunRPC]
	public void Destroy (int id)
	{
		if ( Instance == null )
		{
			return;
		}

		m_Projectiles.RemoveAll (x => x == null);

		Projectile projectile = m_Projectiles.Find (x => x.ID == id);


		if ( projectile != null )
		{
			m_Projectiles.Remove (projectile);
			m_Pool.Reserve (projectile.GetComponent<PooledObject> ());
		}
	}
}
