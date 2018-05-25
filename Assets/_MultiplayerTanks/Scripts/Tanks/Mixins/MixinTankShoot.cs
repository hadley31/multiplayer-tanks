using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixinTankShoot : MonoBehaviour
{
	public float fireRate = 0.018f;

	private float fireTimer = 0;

	private void Update ()
	{
		fireTimer -= Time.deltaTime;
	}

	public void Shoot ()
	{
		if ( !CanShoot () )
			return;


		Debug.Log ("MixinTankShoot::Shoot()");


		// Implement shooting mechanics


		fireTimer = fireRate;
	}

	protected virtual bool CanShoot ()
	{
		return fireTimer <= 0;
	}
}
