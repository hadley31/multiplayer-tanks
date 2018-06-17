using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankLandmine : TankBase
{
	public Landmine landminePrefab;
	public int maxLandmines = 2;
	public float useCooldown = 5;
	public float landmineRechargeCooldown = 20;

	public float fuse = 8;
	public int damage = 1000;
	public float radius = 2;

	public int Landmines
	{
		get;
		private set;
	}

	private float m_lastUseTime;
	private float m_rechargeTimer;

	private void Start ()
	{
		Landmines = maxLandmines;
	}

	private void Update ()
	{
		if ( photonView.isMine == false )
		{
			return;
		}

		if ( Tank.IsAlive == false )
		{
			return;
		}

		m_rechargeTimer -= Time.deltaTime;

		if ( Landmines < maxLandmines )
		{
			m_rechargeTimer -= Time.deltaTime;

			if ( m_rechargeTimer < 0 )
			{
				Landmines++;
				m_rechargeTimer = landmineRechargeCooldown;
			}
		}
	}

	public void Use ()
	{
		if ( photonView.isMine == false )
		{
			return;
		}

		if ( Tank.IsAlive == false )
		{
			return;
		}

		if ( Landmines <= 0 && m_rechargeTimer > 0 )
		{
			return;
		}

		int id = LandmineManager.GetNextID ();

		LandmineManager.Instance.SpawnNewRPC (transform.position, fuse, damage, radius, photonView.viewID, id, PhotonNetwork.time);

		Landmines--;
		m_rechargeTimer = useCooldown;
	}
}
