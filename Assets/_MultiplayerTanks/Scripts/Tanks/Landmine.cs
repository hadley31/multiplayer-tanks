using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (EntityHealth))]
public class Landmine : Entity, IProjectileInteractive, IDestroyable
{
	public Transform explosion;
	public float fuseTime;
	public float radius;
	public int damage;

	protected Material material;
	protected float fuseTimer;
	protected float colorSwitchTime, colorSwitchTimer;
	protected bool isColoredRed;

	protected void Start ()
	{
		this.fuseTimer = fuseTime;
		this.material = GetComponent<Renderer> ().material;
	}

	protected void Update ()
	{
		fuseTimer -= Time.deltaTime;
		colorSwitchTimer -= Time.deltaTime;

		if ( colorSwitchTimer <= 0 )
		{
			isColoredRed = !isColoredRed;
			material.color = isColoredRed ? Color.red : Color.yellow;

			colorSwitchTime = fuseTimer / 10;
			colorSwitchTimer = colorSwitchTime;
		}

		if ( fuseTimer <= 0 )
		{
			DestroyObject ();
			fuseTimer = this.fuseTime;
		}
	}

	public void OnProjectileInteraction (Projectile p)
	{
		DestroyObject ();
		p.DestroyObject ();
	}

	public void DestroyObject ()
	{
		EntityHealth ourHealth = GetComponent<EntityHealth> ();
		Collider[] colliders = Physics.OverlapSphere (transform.position, radius);
		foreach ( Collider c in colliders )
		{
			EntityHealth h = c.GetComponent<EntityHealth> ();

			if ( h != null && h != ourHealth )
			{
				print (h.gameObject.name);
				h.Decrease (damage);
			}
		}

		Destroy (gameObject);
	}
}
