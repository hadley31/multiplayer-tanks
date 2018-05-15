using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LandmineInfo
{
	public int damage;
	public float radius;
	public float fuseTime;

	public LandmineInfo (int damage = 100, float radius = 3f, float fuseTime = 3f)
	{
		this.damage = damage;
		this.radius = radius;
		this.fuseTime = fuseTime;
	}

	public LandmineInfo Clone ()
	{
		return new LandmineInfo (damage, radius, fuseTime);
	}
}