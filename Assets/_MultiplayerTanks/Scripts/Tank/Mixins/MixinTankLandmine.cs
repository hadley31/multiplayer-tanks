using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixinTankLandmine : MonoBehaviour
{
	public Landmine landminePrefab;
	public int landmines = 2;
	public int maxLandmines = 2;
	public float landmineUseCooldown = 5;
	public float landmineRechargeCooldown = 20;

	protected float landmineUseTimer = 0;
	protected float landmineRechargeTimer = 0;

	private void Update ()
	{
		landmineUseTimer -= Time.deltaTime;

		if ( landmines < maxLandmines )
		{
			landmineRechargeTimer -= Time.deltaTime;

			if ( landmineRechargeTimer < 0 )
			{
				landmines++;
				landmineRechargeTimer = landmineRechargeCooldown;
			}
		}
	}

	public void Use ()
	{
		if ( !CanUse () )
			return;


		Debug.Log ("MixinTankLandmine::Use()");


		Instantiate (landminePrefab, transform.position, Quaternion.identity);


		landmines--;
		landmineUseTimer = landmineUseCooldown;
	}

	protected virtual bool CanUse ()
	{
		return landmines > 0 && landmineUseTimer <= 0;
	}
}
