using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTheTank_Tank : TankBase
{
	public float rotateSpeed;

	private float m_LookAngle;

	private void Update ()
	{
		Shooting.Shoot ();
		Movement.SetLookTarget (m_LookAngle);

		m_LookAngle += Time.deltaTime * rotateSpeed;
	}
}
