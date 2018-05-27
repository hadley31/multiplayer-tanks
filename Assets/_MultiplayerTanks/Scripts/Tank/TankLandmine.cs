using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankLandmine : Photon.MonoBehaviour
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
	private int m_landmineID;

	private List<Landmine> m_Landmines = new List<Landmine> ();

	private void Start ()
	{
		Landmines = maxLandmines;
	}

	private void Update ()
	{
		if (!photonView.isMine)
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
		if ( Landmines <= 0 && m_rechargeTimer > 0 )
		{
			return;
		}

		if (NetworkManager.OfflineMode)
		{
			CreateLandmine (transform.position, fuse, damage, radius, m_landmineID, PhotonNetwork.time);
		}
		else
		{
			photonView.RPC ("CreateLandmine", PhotonTargets.All, transform.position, fuse, damage, radius, m_landmineID, PhotonNetwork.time);
		}


		Landmines--;
		m_landmineID++;
		m_rechargeTimer = useCooldown;
	}

	[PunRPC]
	private void CreateLandmine (Vector3 position, float fuse, int damage, float radius, int id, double createTime)
	{
		Landmine newLandmine = Instantiate (landminePrefab);

		fuse -= (float) ( PhotonNetwork.time - createTime );

		newLandmine.SetPosition (position);
		newLandmine.SetFuse (fuse);
		newLandmine.SetRadius (radius);
		newLandmine.SetDamage (damage);
		newLandmine.SetID (id);
		newLandmine.SetOwner (GetComponent<Tank> ());

		m_Landmines.Add (newLandmine);
	}

	public void DestroyLandmineRPC (int id)
	{
		if ( NetworkManager.OfflineMode )
		{
			DestroyLandmine (id);
		}
		else
		{
			photonView.RPC ("DestroyLandmine", PhotonTargets.All, id);
		}
	}

	[PunRPC]
	public void DestroyLandmine (int id)
	{
		m_Landmines.RemoveAll (x => x == null);

		Landmine landmine = m_Landmines.Find (x => x.ID == id);

		if ( landmine != null )
		{
			Destroy (landmine.gameObject);
			m_Landmines.Remove (landmine);
		}
	}
}
