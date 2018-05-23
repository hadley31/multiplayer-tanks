using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Tank))]
public class TankController : MonoBehaviour
{
	protected Tank tank;

	public float fireRate = 0.018f;

	public float moveSpeed = 1;
	public float turnSpeed = 10;
	public float topRotateSpeed = 10;

	public float boost = 0;
	public float boostSpeedMultiplier = 5;
	public float maxBoost = 1;
	public float boostUseSpeed = 3;
	public float boostRegainSpeed = 0.2f;

	public int landmines = 2;
	public int maxLandmines = 2;
	public float landmineUseCooldown = 5;
	public float landmineRechargeCooldown = 20;

	protected float fireTimer = 0;
	protected float landmineUseTimer = 0;
	protected float landmineRechargeTimer = 0;

	#region Monobehaviors

	protected virtual void Awake ()
	{
		this.tank = GetComponent<Tank> ();
	}

	protected virtual void Update ()
	{
		fireTimer -= Time.deltaTime;
		landmineUseTimer -= Time.deltaTime;
	}

	#endregion

	protected virtual Vector3 GetTargetPoint ()
	{
		return Vector3.zero;
	}

	protected virtual bool CanLayLandmine ()
	{
		return landmines > 0 && landmineUseTimer <= 0;
	}

	protected virtual bool CanShoot ()
	{
		return fireTimer <= 0;
	}
}
