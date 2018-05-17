using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProjectileInfo
{
	public int bounces;
	public int damage;
	public float radius;
	public float moveSpeed;
	public float lifeSpan;

	public ProjectileInfo (float moveSpeed = 10, int damage = 100, int bounces = 1, float radius = 0.075f, float lifeSpan = 10f)
	{
		this.bounces = bounces;
		this.damage = damage;
		this.moveSpeed = moveSpeed;
		this.radius = radius;
		this.lifeSpan = lifeSpan;
	}

	public ProjectileInfo Clone ()
	{
		return new ProjectileInfo (moveSpeed, damage, bounces, radius, lifeSpan);
	}
}
