using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_TankInput_Test : TankBase
{
	public float lookSpeed = 5;

	private float m_LookRotation = 0;

	private void Update ()
	{
		LookRotate ();
		Shooting.Shoot ();
	}

	private void LookRotate ()
	{
		Movement.SetLookTarget (m_LookRotation);

		m_LookRotation += Time.deltaTime * lookSpeed;
	}
}
