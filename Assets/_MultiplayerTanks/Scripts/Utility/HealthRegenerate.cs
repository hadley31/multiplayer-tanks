using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRegenerate : MonoBehaviour
{
	public Health health;

	public int amount = 1;
	public float rate = 1;

	private float m_Timer = 0;

	private void Update ()
	{
		m_Timer += Time.deltaTime;

		if (m_Timer > rate)
		{
			health.DecreaseRPC (-amount);
			m_Timer = 0;
		}
	}
}
