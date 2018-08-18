using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (ObjectPool), typeof (PhotonView))]
public class LandmineManager : Photon.MonoBehaviour
{
	public static LandmineManager Instance
	{
		get;
		private set;
	}

	public static int GetNextID ()
	{
		return Instance.m_LandmineID++;
	}

	#region Private Fields

	private int m_LandmineID = 0;
	private ObjectPool m_Pool;
	private List<Landmine> m_Landmines = new List<Landmine> ();

	#endregion

	#region Monobehaviours

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

	#endregion

	#region Spawn / Destroy

	public void SpawnNewRPC (Vector3 position, float fuse, int damage, int health, float radius, int viewID, int mineID, double createTime)
	{
		photonView.RPC ("SpawnNew", PhotonTargets.All, position, fuse, damage, health, radius, viewID, mineID, createTime);
	}

	[PunRPC]
	public void SpawnNew (Vector3 position, float fuse, int damage, int health, float radius, int viewID, int mineID, double createTime)
	{
		Landmine landmine = m_Pool.Spawn<Landmine> ();

		float dt = (float) ( PhotonNetwork.time - createTime );
		fuse -= dt;

		landmine.Health.SetMaxValue (health, true);
		landmine.transform.SetParent (transform);

		landmine.SetFuse (fuse);
		landmine.SetPosition (position);
		landmine.SetDamage (damage);
		landmine.SetRadius (radius);
		landmine.SetSender (viewID);
		landmine.SetID (mineID);

		m_Landmines.Add (landmine);

		m_LandmineID = mineID + 1;
	}

	public void DestroyRPC (int id)
	{
		if ( NetworkManager.IsMasterClient == false )
		{
			return;
		}

		photonView.RPC ("Destroy", PhotonTargets.All, id);
	}

	[PunRPC]
	public void Destroy (int id)
	{
		if ( Instance == null )
		{
			return;
		}

		m_Landmines.RemoveAll (x => x == null);

		Landmine landmine = m_Landmines.Find (x => x.ID == id);


		if ( landmine != null )
		{
			landmine.SpawnExplosion ();

			m_Landmines.Remove (landmine);
			m_Pool.Reserve (landmine.GetComponent<PooledObject> ());
		}
	}

	#endregion
}
