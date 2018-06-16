using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_TankInput_Test : TankBase
{
	public float moveRandomness = 10;
	public float lookSpeed = 5;

	private float m_LookRotation = 0;

	private void Update ()
	{
		float angle = Mathf.PerlinNoise ((transform.position.x + Time.time * 0.0001f) * moveRandomness, (transform.position.z + Time.time * 0.0001f) * moveRandomness);

		angle = Mathf.Lerp (0, Mathf.PI * 2, angle);

		float x = Mathf.Cos (angle);
		float z = Mathf.Sin (angle);

		Vector3 direction = new Vector3 (x, 0, z);

		Movement.SetTargetDirection (direction);

		LookRotate ();
		//Shooting.Shoot ();
	}

	private void LookRotate ()
	{
		Movement.SetLookTarget (m_LookRotation);

		m_LookRotation += Time.deltaTime * lookSpeed;
	}
}
