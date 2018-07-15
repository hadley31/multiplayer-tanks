using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandmineExplosion : MonoBehaviour
{
	public float rotateSpeed = 10;
	public float growSpeed = 2;

	private float m_Radius;
	private float m_Lifespan;
	private float m_Lifetime;

	public void Prime (float radius, float lifespan)
	{
		m_Radius = radius;
		m_Lifespan = lifespan;
		m_Lifetime = 0;

		Destroy (gameObject, m_Lifespan);
	}


	private void Update ()
	{
		m_Lifetime += Time.deltaTime;
		transform.localScale = Vector3.one * Mathf.Lerp (0, m_Radius * 2, m_Lifetime / m_Lifespan * growSpeed);

		transform.Rotate (0, rotateSpeed, 0);
	}

	public static LandmineExplosion Get (int id)
	{
		switch ( id )
		{
			case 0:
				return Resources.Load<LandmineExplosion> ("Landmine_Explosion");
			default:
				return null;
		}
	}
}
